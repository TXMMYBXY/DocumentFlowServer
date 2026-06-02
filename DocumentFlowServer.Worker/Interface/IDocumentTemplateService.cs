using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentFlowServer.Worker.Interface;

public interface IDocumentTemplateService
{
    Task<byte[]> FillTemplateAsync(string templatePath, Dictionary<string, string> fields);
}
