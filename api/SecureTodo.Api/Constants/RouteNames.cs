namespace SecureTodo.Api.Constants;

public static class RouteNames
{
    public static class Tasks
    {
        public const string Tag = "Tasks";
        private const string Base = "tasks";
        public const string GetById = $"{Base}/{{id}}";
        public const string GetAll = $"{Base}";
        public const string Create = $"{Base}";
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string Completion = $"{Base}/{{id}}/completion";
    }
}