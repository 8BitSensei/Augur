using LiteDbTest.Models;
using System.Collections.Generic;

namespace LiteDbTest.Interfaces
{
    public interface IDataService
    {
        Artifact GetArtifact(int id);
        Artifact GetArtifact(string text, string source);
        Entity GetEntity(string name);
        (int, List<Connection>) GetEntityConections(string entityName);
        Entity UpdateEntityOccurences(string name, int artifactId);
    }
}