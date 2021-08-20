using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace TestDataDefinitionFramework.Docker
{
    public static class DockerRunner
    {
        public static async Task EnsureContainerIsRunningAsync(string name, string imageName, int exposePort)
        {
            var client = new DockerClientConfiguration()
                .CreateClient();

            var existingContainer = (
                await client.Containers.ListContainersAsync(new ContainersListParameters
                {
                    All = true,
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        {"name", new Dictionary<string, bool> {{name, true}}}
                    }
                })).SingleOrDefault();

            if (existingContainer != null &&
                existingContainer.State.Equals("running", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var containerId = existingContainer?.ID;

            if (containerId == null)
            {
                containerId = (
                    await client.Containers.CreateContainerAsync(new CreateContainerParameters
                    {
                        Name = name,
                        Image = imageName,
                        ExposedPorts = new Dictionary<string, EmptyStruct>
                        {
                            {exposePort.ToString(), default}
                        },
                        HostConfig = new HostConfig
                        {
                            AutoRemove = true,
                            Init = true,
                            PortBindings = new Dictionary<string, IList<PortBinding>>
                            {
                                {
                                    exposePort.ToString(), new[]
                                    {
                                        new PortBinding {HostPort = exposePort.ToString()}
                                    }
                                }
                            }
                        }
                    })).ID;
            }
         
            await client.Containers.StartContainerAsync(containerId,
                    new ContainerStartParameters());
        }
    }
}