namespace API.DTOs
{
    public class MemberUpdateDTO
    {
        // 1. we'll give the users update only these fields:
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        //2. because this information is not asked when they register, no need for validation here, they can all be blanks
        //3. because this object need to be mapped to out User Entity we'll use Auto Mapper, but this time we'll go the other way:
        // go to AutoMapperProfiles.cs
    }
}