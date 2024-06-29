namespace EnglishMaster.Shared.Dto.Response
{
    public class LoginResponseDto
    {
        public LoginResponseDto(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
        }
        public string Token { get; }
        public DateTime Expires { get; }
    }
}