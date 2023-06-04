namespace Elearninig.Base.API.Variables
{
    public static class EnvVariables
    {
        //  Environment variables are key-value pairs
        //  that are set in the operating system or hosting environment and can be accessed by applications running on that system
        //  HINT: you can replace "ASPNETCORE_ENVIRONMENT" with any name you prefer for your environment variable. 
        public static string ENVIRONMENT_NAME = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
