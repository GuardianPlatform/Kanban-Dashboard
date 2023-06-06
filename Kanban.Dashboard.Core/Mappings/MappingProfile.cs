using AutoMapper;
using Kanban.Dashboard.Core.Entities;
using System.Threading.Tasks;
using Kanban.Dashboard.Core.Dtos;

namespace Kanban.Dashboard.Core.Mappings
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDto>().ReverseMap();
            CreateMap<Column, ColumnDto>().ReverseMap();
            CreateMap<KanbanTask, KanbanTaskDto>().ReverseMap();
            CreateMap<Subtask, SubtaskDto>().ReverseMap();
        }
    }
}
