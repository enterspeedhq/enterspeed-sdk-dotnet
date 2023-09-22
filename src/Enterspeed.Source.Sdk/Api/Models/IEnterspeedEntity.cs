using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace Enterspeed.Source.Sdk.Api.Models
{
    public interface IEnterspeedEntity
    {
        /// <summary>
        /// The id of your source entity. It will become OriginId in Enterspeed
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The type of your source entity. E.g. contentPage, product, media
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The URL of your source entity. Typically used if your source entity is a CMS page or anything else with a URL
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Incoming redirects on your source entity. Typically used if your source entity is a CMS page or anything else with a URL
        /// </summary>
        string[] Redirects { get; }

        /// <summary>
        /// The parent id of your source entity. Used if your source entities has a hierarchical structure. It will become OriginParentId in Enterspeed
        /// </summary>
        string ParentId { get; }

        /// <summary>
        /// Must be a string as a serialized json object containing data properties for your source entity<br/>
        /// or <br/>
        /// Any object containing data properties for your source entity
        /// </summary>
        object Properties { get; }
    }

    public interface IEnterspeedEntity<T>
    {
        /// <summary>
        /// The id of your source entity. It will become OriginId in Enterspeed
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The type of your source entity. E.g. contentPage, product, media
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The URL of your source entity. Typically used if your source entity is a CMS page or anything else with a URL
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Incoming redirects on your source entity. Typically used if your source entity is a CMS page or anything else with a URL
        /// </summary>
        string[] Redirects { get; }

        /// <summary>
        /// The parent id of your source entity. Used if your source entities has a hierarchical structure. It will become OriginParentId in Enterspeed
        /// </summary>
        string ParentId { get; }

        /// <summary>
        /// Must be a string as a serialized json object containing data properties for your source entity<br/>
        /// or <br/>
        /// Any object containing data properties for your source entity
        /// </summary>
        T Properties { get; }
    }
}
