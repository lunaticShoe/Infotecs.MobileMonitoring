using System;

namespace Infotecs.MobileMonitoring.Exceptions;

public class ElementDoesNotExistsException: Exception
{
    public ElementDoesNotExistsException(Guid id) : 
        base($"Element with id = {id} does not exists")
    {
        
    }
}
