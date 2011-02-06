using Gymnastika.Data.Migration.Commands;

namespace Gymnastika.Data.Migration.Interpreters {
    /// <summary>
    /// This interface can be implemented to provide a data migration behavior
    /// </summary>
    public interface ICommandInterpreter<T> : ICommandInterpreter 
        where T : ISchemaBuilderCommand 
    {
        string[] CreateStatements(T command);
    }

    public interface ICommandInterpreter 
    {
        string DataProvider { get; }
    }
}
