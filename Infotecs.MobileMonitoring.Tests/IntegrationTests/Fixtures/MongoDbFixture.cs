using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Migrator;

namespace Infotecs.MobileMonitoring.Tests.IntegrationTests.Fixtures;

public class MongoDbFixture : IDisposable
{
    private const string Login = "root";
    private const string Password = "4321";
    private const string InternalPort = "27017";
    private readonly DockerClient client;
    private readonly string containerId;
    
    public MongoDbFixture()
    {
        // Создать контейнер и подключиться к монго
        client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
            .CreateClient();

        var (container, port) = GetContainer(client, "mongo", "latest")
            .GetAwaiter().GetResult();

        containerId = container.ID;
        Context = new MongoDbContext($"mongodb://{Login}:{Password}@host.docker.internal:{port}/");
        
        MongoDbMapper.Map();
        
        // Миграции
        var migrator = new MongoDbMigrator(Context);
        migrator.Migrate();
    }
    
    public void Dispose()
    {
        client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters
        {
            Force = true
        });
    }
 
    public IMongoDbContext Context { get; }
    
    private async Task<(CreateContainerResponse, string)> GetContainer(DockerClient client, string image, string tag)
    {
        var hostPort = new Random((int)DateTime.UtcNow.Ticks).Next(10000, 12000);
        
        //look for image
        var images = await client.Images.ListImagesAsync(new ImagesListParameters
            {
                //MatchName = $"{image}:{tag}",
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["reference"] = new Dictionary<string, bool>
                    {
                        [$"{image}:{tag}"] = true
                    }
                }
            },
            CancellationToken.None);

        //check if container exists
        var pgImage = images.FirstOrDefault();
        if (pgImage == null)
            throw new Exception($"Docker image for {image}:{tag} not found.");

        //create container from image
        var container = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                //User = "postgres",
                Env = new List<string>()
                {
                    "MONGO_DATA_DIR=/data/db",
                    $"MONGO_INITDB_ROOT_USERNAME={Login}",
                    $"MONGO_INITDB_ROOT_PASSWORD={Password}"
                },
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    [InternalPort] = new EmptyStruct()
                },
                HostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>()
                    {
                        [InternalPort] = new List<PortBinding>()
                            { new PortBinding() { HostIP = "0.0.0.0", HostPort = $"{hostPort}" } }
                    }
                },
                Image = $"{image}:{tag}",
            },
            CancellationToken.None);

        if (!await client.Containers.StartContainerAsync(container.ID,
                new ContainerStartParameters()
                {
                    DetachKeys = $"d={image}"
                },
                CancellationToken.None))
        {
            throw new Exception($"Could not start container: {container.ID}");
        }

        var count = 10;
        Thread.Sleep(5000);
        var containerStat = await client.Containers.InspectContainerAsync(container.ID, CancellationToken.None);
        while (!containerStat.State.Running && count-- > 0)
        {
            Thread.Sleep(1000);
            containerStat = await client.Containers.InspectContainerAsync(container.ID, CancellationToken.None);
        }
        
        return (container, $"{hostPort}");
    }
}
