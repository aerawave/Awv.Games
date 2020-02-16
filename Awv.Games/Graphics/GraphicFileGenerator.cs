using Awv.Automation.Generation;
using Awv.Automation.Generation.Interface;
using System;
using System.IO;
using System.Linq;

namespace Awv.Games.Graphics
{
    public class GraphicFileGenerator : IGenerator<IGraphic>
    {
        protected FilePathGenerator FileGenerator { get; set; }

        public GraphicFileGenerator(string directory)
        {
            FileGenerator = new FilePathGenerator(directory);
        }

        public virtual IGraphic Generate(IRNG random)
        {
            var file = FileGenerator.Generate(random);

            return file == null ? null : new Graphic(file);
        }

        public IGraphic FindSpecific(string name)
        {
            var lowerName = name.ToLower();
            var file = FileGenerator.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath).ToLower() == lowerName);

            return file == null ? null : new Graphic(file);
        }
    }
}
