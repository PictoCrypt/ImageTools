using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using FunctionLib.Helper;
using FunctionLib.Steganography.Base;
using Newtonsoft.Json;

namespace ImageToolApp.Models
{
    public class Settings
    {
        private static Settings mInstance;
        private static JsonSerializer mSerializer;

        private Settings(IList<SymmetricAlgorithm> encryptionMethods = null,
            IList<SteganographicAlgorithmImpl> steganographicMethods = null)
        {
            mSerializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.Auto};

            LoadConfig();
            EncryptionMethods = encryptionMethods ?? AlgorithmCollector.GetAllAlgorithm<SymmetricAlgorithm>();
            SteganographicMethods = steganographicMethods ??
                                    AlgorithmCollector.GetAllAlgorithm<SteganographicAlgorithmImpl>();
        }

        public static Settings Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new Settings();
                }
                return mInstance;
            }
        }

        public IList<SteganographicAlgorithmImpl> SteganographicMethods { get; }

        public IList<SymmetricAlgorithm> EncryptionMethods { get; }

        public string Password { get; private set; } = string.Empty;

        public SymmetricAlgorithm SelectedEncryptionMethod { get; private set; }

        public SteganographicAlgorithmImpl SelectedSteganographicMethod { get; private set; }

        public string DefaultPath { get; private set; }

        public void LoadConfig()
        {
            var file = Path.Combine(Constants.AppData, "Config.json");
            if (!File.Exists(file))
            {
                return;
            }

            Config config;
            using (var sr = new StreamReader(File.OpenRead(file)))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    config = mSerializer.Deserialize(reader, typeof (Config)) as Config;
                }
            }
            if (config != null)
            {
                Password = config.Password;
                SelectedEncryptionMethod = config.Crypto;
                SelectedSteganographicMethod = config.Stego;
                DefaultPath = config.DefaultPath;
            }
        }

        private SteganographicAlgorithmImpl CreateClassFromType(Type steganographicAlgorithm)
        {
            return (SteganographicAlgorithmImpl) Activator.CreateInstance(steganographicAlgorithm);
        }

        internal void Save(string password, SymmetricAlgorithm selectedEncryptionMethod,
            SteganographicAlgorithmImpl selectedSteganographicMethod,
            string standardPath)
        {
            Password = password;
            SelectedEncryptionMethod = selectedEncryptionMethod;
            SelectedSteganographicMethod = selectedSteganographicMethod;
            DefaultPath = standardPath;

            var config = new Config(DefaultPath, Password, SelectedEncryptionMethod, SelectedSteganographicMethod);
            var file = Path.Combine(Constants.AppData, Constants.ApplicationName + ".json");
            using (var sw = new StreamWriter(File.Create(file)))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    mSerializer.Serialize(writer, config);
                }
            }
        }
    }
}