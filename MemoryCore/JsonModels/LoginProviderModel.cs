namespace MemoryCore.JsonModels
{
    /// <summary>
    /// Model for data sent to provider login function.
    /// </summary>
    /// <seealso cref="MemoryCore.JsonModels.ValidatableModel" />
    public class LoginProviderModel : ValidatableModel
    {
        /// <summary>
        /// The provider to log in through.
        /// </summary>
        /// <value>The provider enum entry.</value>
        public LoginProvider Provider { get; set; }

        /// <summary>
        /// The access token obtained on the client.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; set; }
    }
}