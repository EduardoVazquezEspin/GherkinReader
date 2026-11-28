using GherkinReader.model;

namespace GherkinReader;

public interface IGherkinReader
{
    public void LoadFile(string filepath);
    public IEnumerable<IGherkinTest> YieldTests();
}