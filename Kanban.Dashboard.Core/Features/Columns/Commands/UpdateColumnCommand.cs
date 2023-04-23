﻿using AutoMapper;
using Kanban.Dashboard.Core.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kanban.Dashboard.Core.Features.Columns.Commands
{
    public class UpdateColumnCommand : IRequest
    {
        public Guid BoardId { get; set; }
        public Guid Id { get; set; }
        public ColumnDto Column { get; set; }
    }

    public class UpdateColumnHandler : IRequestHandler<UpdateColumnCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateColumnHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
        {
            var board = await _context.Boards.AnyAsync(x => x.Id == request.BoardId, cancellationToken);
            if (board == false)
                throw new Exception("Board not found.");

            var column = await _context.Columns.FindAsync(request.Id, cancellationToken);
            if (column == null)
                throw new Exception("Column not found.");

            _mapper.Map(request.Column, column);
            _context.Columns.Update(column);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
