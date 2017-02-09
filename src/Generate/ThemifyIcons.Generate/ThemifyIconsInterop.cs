using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ExCSS;
using Newtonsoft.Json;
using ThemifyIcons.Generate.Properties;

namespace ThemifyIcons.Generate
{
    public class ThemifyIconsInterop
    {
        private readonly IconContainer _iconContainer;

        public ThemifyIconsInterop(string bowerJson, string configCss)
        {
            if (!File.Exists(bowerJson))
                throw new FileNotFoundException($"'{bowerJson}' file could not be found");

            if (!File.Exists(configCss))
                throw new FileNotFoundException($"'{configCss}' file could not be found");

            Container = new ConfigContainer();

            using (var fileStream = new FileStream(bowerJson, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var cfg = streamReader.ReadToEnd();
                    dynamic bower = JsonConvert.DeserializeObject<ExpandoObject>(cfg);
                    Container.ThemifyIcons.DocBlob = bower.version.ToString();
                    Container.ThemifyIcons.Url = bower.homepage.ToString();
                    if (bower.keywords.Count > 0)
                        Container.ThemifyIcons.Tagline = string.Join(",", bower.keywords);
                    if (bower.license.Count > 0)
                        Container.ThemifyIcons.License = string.Join(",", bower.license);

                    if (bower.authors.Count > 0)
                    {
                        Container.ThemifyIcons.Authors = new List<Author>();
                        foreach (dynamic author in bower.authors)
                        {
                            var str = author.ToString() as string;
                            if (str == null)
                                continue;
                            var infos = str.Split('<');
                            Container.ThemifyIcons.Authors.Add(new Author { Name = infos[0].Trim(), Contact = infos[1].Trim('<', '>').Trim() });
                            if (bower.repository != null && bower.repository.type != null && bower.repository.type == "git")
                            {
                                Container.ThemifyIcons.Github = new Github();
                                Container.ThemifyIcons.Github.Url = bower.repository.url.ToString();
                            }
                        }
                    }
                }
            }

            var parser = new Parser();
            _iconContainer = new IconContainer();
          
            using (var fileStream = new FileStream(configCss, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var css = streamReader.ReadToEnd();
                    var styleSheet = parser.Parse(css);

                    foreach (var styleRule in styleSheet.StyleRules.Where(s => s.Value.StartsWith(".ti-")))
                    {
                        var selector = styleRule.Value;
                        var id = selector.Replace(".ti-", string.Empty).Replace(":before", string.Empty);
                        var strUnicode = styleRule.Declarations.First(d => d.Name.Equals("content", StringComparison.InvariantCultureIgnoreCase)).Term.ToString();
                        var unicode = $@"{(ushort) strUnicode[1]:x4}";
                        var iconEntry = new IconEntry {Id = id, Unicode = unicode};
                        _iconContainer.Icons.Add(iconEntry);
                    }
                }
            }
        }

        public IEnumerable<IconEntry> Items => _iconContainer.Icons;

        public ThemifyIconsConfig Config => Container.ThemifyIcons;

        public ConfigContainer Container
        {
            get;
        }

        #region [ Deserialize ]

        [UsedImplicitly]
        public class ConfigContainer
        {
            public ConfigContainer()
            {
                ThemifyIcons = new ThemifyIconsConfig();
            }

            public ThemifyIconsConfig ThemifyIcons { get; set; }
        }

        [UsedImplicitly]
        public class ThemifyIconsConfig
        {
            public string DocBlob { get; set; }
            public string Url { get; set; }

            public string Tagline { get; set; }
            public string License { get; set; }

            public List<Author> Authors { get; set; }

            public Github Github { get; set; }
        }

        [UsedImplicitly]
        public class IconContainer
        {
            public IconContainer()
            {
                Icons = new List<IconEntry>();
            }

            public List<IconEntry> Icons { get; set; }
        }

        [UsedImplicitly]
        public class Author
        {
            public string Name { get; set; }

            public string Contact { get; set; }
        }

        [UsedImplicitly]
        public class Github
        {
            public string Url { get; set; }
        }

        [UsedImplicitly]
        public class IconEntry
        {
            private static readonly Regex REG_PROP = new Regex(@"\([^)]*\)");

            public string Id { get; set; }
            public string Unicode { get; set; }

            private string _name = null;

            public string Name
            {
                get
                {
                    if (string.IsNullOrEmpty(_name))
                    {
                        _name = Safe(Id);
                    }
                    return _name;
                }
            }

            public string Safe(string text)
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture;
                var textInfo = cultureInfo.TextInfo;

                if (text.EndsWith("-o") || text.Contains("-o-"))
                    text = text.Replace("-o", "-outline");

                var stringBuilder = new StringBuilder(textInfo.ToTitleCase(text.Replace("-", " ")));

                stringBuilder
                    .Replace("-", string.Empty).Replace("/", "_")
                    .Replace(" ", string.Empty).Replace(".", string.Empty)
                    .Replace("'", string.Empty);

                var matches = REG_PROP.Matches(stringBuilder.ToString());
                stringBuilder = new StringBuilder(REG_PROP.Replace(stringBuilder.ToString(), string.Empty));
                var hasMatch = false;

                for (var i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    if (match.Value.IndexOf("Hand", StringComparison.InvariantCultureIgnoreCase) > -1)
                    {
                        hasMatch = true;
                        break;
                    }
                }

                if (hasMatch)
                {
                    stringBuilder.Insert(0, "Hand");
                }

                if (char.IsDigit(stringBuilder[0]))
                    stringBuilder.Insert(0, '_');

                return stringBuilder.ToString();
            }
        }

        #endregion
    }
}

