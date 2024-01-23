namespace Angular_Authentication_with_JWT.Dto_s
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Error { get; set; }
    }
}
