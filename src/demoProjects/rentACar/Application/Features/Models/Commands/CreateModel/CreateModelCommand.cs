using Application.Features.Brands.Dtos;
using Application.Features.Brands.Rules;
using Application.Features.Models.Dtos;
using Application.Features.Models.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Commands.CreateModel
{
    public class CreateModelCommand:IRequest<CreatedModelDto>
    {
        public string Name { get; set; }
       
      
        public class CreateModelCommandHandle : IRequestHandler<CreateModelCommand, CreatedModelDto>
        {
            private readonly IModelRepository _modelRepository;
            private readonly IMapper _mapper;
            private readonly ModelBusinessRules _modelBusinessRules;
            public CreateModelCommandHandle(IModelRepository modelRepository, IMapper mapper, ModelBusinessRules modelBusinessRules )
            {
                _modelRepository = modelRepository;
                _mapper = mapper;
                _modelBusinessRules = modelBusinessRules;

            }

            public async Task<CreatedModelDto> Handle(CreateModelCommand request, CancellationToken cancellationToken)
            {
                await _modelBusinessRules.ModelNameCanNotBeDuplicatedWhenInserted(request.Name);

                Model mappedModel = _mapper.Map<Model>(request);
                Model createdModel = await _modelRepository.AddAsync(mappedModel);
                CreatedModelDto createdModelDto = _mapper.Map<CreatedModelDto>(createdModel);

                return createdModelDto;
            }
        }

    }
}
