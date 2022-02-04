using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        //1. we'll need to have getters for our repositories
        IUserRepository UserRepository { get;  }
        IMessageRepository MessageRepository { get;  }

        ILikesRepository LikesRepository { get;  }

        //2. a method to save all of our changes
        Task<bool> Complete();


        //3. and just onw more method to see if EF has been tracking or has any changes (we'll use this in one specific place, you'll see)
        bool HasChanges();

        //4. create and go to the implementation of this interface, got to 

    }
}