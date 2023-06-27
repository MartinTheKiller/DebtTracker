using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Text;
using Xunit.Abstractions;

namespace DebtTracker.Common.Tests;

public class XUnitTestOutputConverter : TextWriter
{
    private readonly ITestOutputHelper _output;
    public XUnitTestOutputConverter(ITestOutputHelper output) => _output = output;
    public override Encoding Encoding => Encoding.UTF8;

    public override void WriteLine(string? message)
    {
        try
        {
            _output.WriteLine(message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"{nameof(InvalidOperationException)} occurred when trying to write to test's output.");
            Console.WriteLine(e.Message);
            Console.WriteLine($"Tried to write:");
            Console.WriteLine(message);
        }
    }

    public override void WriteLine(string format, params object?[] args)
    {
        try
        {
            _output.WriteLine(format, args);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"{nameof(InvalidOperationException)} occurred when trying to write to test's output.");
            Console.WriteLine(e.Message);
            Console.WriteLine($"Tried to write:");
            Console.WriteLine(format, args);
        }
    }
}