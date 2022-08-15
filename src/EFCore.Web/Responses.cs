namespace EFCore.Web;

public interface IResponse // Marker
{
}

public record TablePerHierarchyResponse(object Result) : IResponse;
public record TablePerTypeResponse(object Result) : IResponse;
public record CascadeResponse(object Result) : IResponse;
