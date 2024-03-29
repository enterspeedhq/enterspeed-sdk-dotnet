﻿namespace Enterspeed.Source.Sdk.Api.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
    }
}