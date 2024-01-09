using BusinessLogicLayer.Models;
using BusinessLogicLayer.ViewModels;

namespace MyBlog.Extentions
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserEditViewModel usereditvm)
        {
            user.LastName = usereditvm.LastName;
            user.FirstName = usereditvm.FirstName;
            user.MiddleName = usereditvm.MiddleName;
            user.Email = usereditvm.Email;
            user.BirthDate = usereditvm.BirthDate;
            user.UserName = usereditvm.UserName;

            return user;
        }
    }
}
