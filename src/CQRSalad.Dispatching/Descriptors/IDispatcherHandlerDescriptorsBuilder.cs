﻿using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.Descriptors
{
    public interface IDispatcherHandlerDescriptorsBuilder
    {
        IEnumerable<HandlerDescriptor> CreateHandlerDescriptors(IEnumerable<TypeInfo> handlerTypes);
    }
}
    