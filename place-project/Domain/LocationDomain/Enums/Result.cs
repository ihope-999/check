namespace place_project.Domain.LocationDomain.Enums
{

    public enum CheckResult
    {
        NotFound,
        Found

    }
    public enum ScoreResult
    {
        Perfect = 100,
        Partial = 80,
        Inconsistent = 60,
        None = 40
    }
}
