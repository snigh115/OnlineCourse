namespace StudentRegisteration.Models.Class
{
    public class LoginResult
    {
        public bool IsAuthenticated  { get; set; }
        // public TUser User {get ; set ;}
        public User User  { get; set; }
        public string ErrorMessage { get; set; }
        
    }
}