namespace API.Helpers
{
    //1. derive from the pagination class
    public class LikesParams: PaginationParams
    {
        //1. we also need (based on GetUserLikes action in like respsitory)
        public int UserId { get; set; }
        public string Predicate { get; set; }
        
        //2. next we need to update the repository to accept these params
        // * go to ILikesRepository.cs
        
        
        
        
    }
}