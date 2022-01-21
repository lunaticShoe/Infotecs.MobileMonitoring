using System;

namespace Infotecs.MobileMonitoring.Exceptions;

public class ElementAlreadyExistsException : Exception
{
    public ElementAlreadyExistsException(Guid elementId) : 
        base($"Element with id = {elementId} already exists")
    {
        
    }
}
