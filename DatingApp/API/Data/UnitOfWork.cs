using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    //1. now what UOF does for us is creating instances of repositories, like a repositpry factory
    public class UnitOfWork: IUnitOfWork
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // 2. before now we used DI to inject context and mapper, now we'll create them here
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);
        public ILikesRepository LikesRepository => new LikesRepository(_context);


        public async Task<bool> Complete()
        {
            //3. all the changes EF has tracked are saved to the database 
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            //4. just return if ED is tracking something 
            return _context.ChangeTracker.HasChanges();
        }

        //5. now just to insert this as a service to the DI container 
        // * and remove other repos - they are not part of DI anymore
        //6. go to ApplicationServiceExtensions.cs  
    }
}