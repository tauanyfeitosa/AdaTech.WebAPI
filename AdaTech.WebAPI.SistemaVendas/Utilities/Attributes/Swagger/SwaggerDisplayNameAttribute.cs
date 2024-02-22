namespace AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerDisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public SwaggerDisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

}
