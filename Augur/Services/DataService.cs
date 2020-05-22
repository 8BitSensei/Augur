using LiteDB;
using LiteDbTest.Interfaces;
using LiteDbTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDbTest
{
    public class DataService : IDataService
    {
        private LiteDatabase _db;

        public DataService(LiteDatabase db)
        {
            _db = db;
        }

        public Artifact GetArtifact(string text, string source)
        {
            var artifact = new Artifact
            {
                Names = new Dictionary<int, string>(),
                Source = source
            };

            var art = _db.GetCollection<Artifact>("artifacts");

            art.EnsureIndex(x => x.Source);
            var results = art.Query().Where(x => x.Source == source).FirstOrDefault();
            if (results != null)
            {
                return results;
            }

            var names = text.Split(" ");
            foreach (var name in names)
            {
                var searchResult = GetEntity(name);
                artifact.Names.Add(searchResult.id, searchResult.Name);
            }

            art.Insert(artifact);
            var entry = GetArtifact(text, source);
            foreach (var entity in entry.Names.Values)
            {
                UpdateEntityOccurences(entity, entry.id);
            }

            Console.WriteLine("Inserting... ");
            Console.WriteLine("Id: " + entry.id.ToString());
            Console.WriteLine("Name: " + string.Join(" ", entry.Names.Values));
            Console.WriteLine();

            return entry;
        }

        public Artifact GetArtifact(int id)
        {
            var art = _db.GetCollection<Artifact>("artifacts");
            var result = art.FindById(id);
            return result;
        }

        public Entity UpdateEntityOccurences(string name, int artifactId)
        {
            var entity = GetEntity(name);
            entity.Occurences.Add(artifactId);

            var ent = _db.GetCollection<Entity>("entities");
            ent.Update(entity);

            return entity;
        }

        public Entity GetEntity(string name)
        {
            var entity = new Entity
            {
                Occurences = new List<int>(),
                Name = name
            };

            var ent = _db.GetCollection<Entity>("entities");
            ent.EnsureIndex(x => x.Name);
            var results = ent.Query().Where(x => x.Name == entity.Name).FirstOrDefault();
            if (results == null)
            {
                ent.Insert(entity);
                return GetEntity(entity.Name);
            }

            return results;
        }

        public (int, List<Connection>) GetEntityConections(string entityName)
        {
            var entity = GetEntity(entityName);
            var totalOccurences = entity.Occurences.Count;
            var nameOccurencePairs = new List<Connection>();
            foreach (var artifactId in entity.Occurences)
            {
                var artifact = GetArtifact(artifactId);
                var names = artifact.Names;
                foreach (var connectionName in names)
                {
                    if (connectionName.Value != entityName)
                    {
                        if (nameOccurencePairs.Where(x => x.name == connectionName.Value).FirstOrDefault() is Connection connection)
                        {
                            connection.weight += 1;
                        }
                        else
                        {
                            nameOccurencePairs.Add(new Connection
                            {
                                id = connectionName.Key,
                                name = connectionName.Value,
                                weight = 1
                            });
                        }
                    }
                }
            }

            return (totalOccurences, nameOccurencePairs);
        }
    }
}
