using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Congress {
    public class AppSettings {
        // Our isolated storage settings
        IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        const string LocationEnabledKey = "LocationEnabled";

        const bool LocationEnabledDefault = true;

        public AppSettings() {
            try {
                isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (Exception e) {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        public void save() {
            isolatedStore.Save();
        }

        public bool AddOrUpdateValue(string Key, Object value) {
            bool valueChanged = false;

            try {
                if (isolatedStore[Key] != value) {
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            } catch (KeyNotFoundException) {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            } catch (ArgumentException) {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            } catch (Exception e) {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }

            return valueChanged;
        }

        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue) {
            valueType value;

            try {
                value = (valueType) isolatedStore[Key];
            } catch (KeyNotFoundException) {
                value = defaultValue;
            } catch (ArgumentException) {
                value = defaultValue;
            }

            return value;
        }

        public bool LocationEnabled {
            get {
                return GetValueOrDefault<bool>(LocationEnabledKey, LocationEnabledDefault);
            }
            set {
                AddOrUpdateValue(LocationEnabledKey, value);
                save();
            }
        }

    }
}