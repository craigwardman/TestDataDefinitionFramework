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
        public static async Task EnsureContainerIsRunningAsync(string name, string imageName, int exposePort, Dictionary<string, string> environmentVariables = null)
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

            await EnsureImageExists(client, imageName);

            var containerId = existingContainer?.ID;

            if (containerId == null)
            {
                var env = environmentVariables?
                    .Select(e => $"{e.Key}={e.Value}")
                    .ToArray() ?? Array.Empty<string>();
                    
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
                        },
                        Env = env,
                    })).ID;
            }
         
            await client.Containers.StartContainerAsync(containerId,
                    new ContainerStartParameters());
        }

        private static async Task EnsureImageExists(IDockerClient dockerClient, string imageName)
        {
            var imagesListResponses = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    {"reference", new Dictionary<string, bool> {{imageName, true}} }
                }
            }));
            
            var exists = imagesListResponses.Count > 0;

            if (!exists)
            {
                await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
                {
                    FromImage = imageName
                }, 
                new AuthConfig(),
                new Progress<JSONMessage>());
            }
        }
    }
}