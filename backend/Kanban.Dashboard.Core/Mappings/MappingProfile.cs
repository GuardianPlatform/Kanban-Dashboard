using AutoMapper;
using Kanban.Dashboard.Core.Entities;
using Kanban.Dashboard.Core.Dtos;
using Kanban.Dashboard.Core.Dtos.Requests;

namespace Kanban.Dashboard.Core.Mappings
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDto>().ReverseMap();
            CreateMap<Column, ColumnDto>().ReverseMap();
            CreateMap<KanbanTask, KanbanTaskDto>().ReverseMap();
            CreateMap<KanbanTask, SubtaskDto>().ReverseMap();
            CreateMap<KanbanTaskDto, CreateOrUpdateKanbanTaskRequest>().ReverseMap();
            CreateMap<ColumnDto, CreateOrUpdateColumnRequest>().ReverseMap();
            CreateMap<BoardDto, CreateBoardRequest>().ReverseMap();
            CreateMap<BoardDto, UpdateBoardRequest>().ReverseMap();
            CreateMap<ColumnDto, CreateOrUpdateColumnsForBoardRequest>().ReverseMap();
        }
    }
}