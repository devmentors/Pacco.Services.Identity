namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidRoleException : ExceptionBase
    {
        public override string Code => "invalid_role";
        
        public InvalidRoleException(string role) : base($"Invalid role: {role}.")
        {
        }
    }
}