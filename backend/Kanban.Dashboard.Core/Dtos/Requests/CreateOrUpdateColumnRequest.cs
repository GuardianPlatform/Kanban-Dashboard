using System;
using System.Runtime.Serialization;
using AutoMapper.Configuration.Annotations;

namespace Kanban.Dashboard.Core.Dtos.Requests
{
    public class CreateOrUpdateColumnRequest : BaseRequest
    {
        public string Name { get; set; }
        public Guid? BoardId { get; set; }
    }
}
