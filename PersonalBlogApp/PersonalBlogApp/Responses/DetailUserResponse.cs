using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Responses
{
    public class DetailUserResponse
    {
        public UserBaseRequest User { get; set; }
        //public IList<string> UserRoles { get; set; }
        public IList<string> AllRoles { get; set; }
    }
}
