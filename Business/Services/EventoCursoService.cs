using AutoMapper;
using Business.Interfaces;
using Business.TransferObjects;
using Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EventoCursoService : IEventoCursoService
    {
        private readonly IMapper _mapper;
        private readonly IEventoCursoRepository _eventoCursoRepo;

        public EventoCursoService(IMapper mapper,
            IEventoCursoRepository eventoCursoRepo)
        {
            _mapper = mapper;
            _eventoCursoRepo = eventoCursoRepo;
        }

        public async Task<List<EventoCursoDto>> GetAllAsync()
        {
            var result = await _eventoCursoRepo.GetAllAsync();
            return _mapper.Map<List<EventoCursoDto>>(result);
        }
    }
}
