using UnityEngine;
using System.Collections.Generic;

    public class Blackboard : MonoBehaviour
    {
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        // --- Bool ---
        public void SetBool(string key, bool value)
        {
            variables[key] = value;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (variables.TryGetValue(key, out object value))
            {
                return (bool)value;
            }
            return defaultValue;
        }

        // --- Float ---
        public void SetFloat(string key, float value)
        {
            variables[key] = value;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            if (variables.TryGetValue(key, out object value))
            {
                return (float)value;
            }
            return defaultValue;
        }

        // --- Int ---
        public void SetInt(string key, int value)
        {
            variables[key] = value;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (variables.TryGetValue(key, out object value))
            {
                return (int)value;
            }
            return defaultValue;
        }

        // --- String ---
        public void SetString(string key, string value)
        {
            variables[key] = value;
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (variables.TryGetValue(key, out object value))
            {
                return (string)value;
            }
            return defaultValue;
        }

        // --- Object ---
        public void SetObject(string key, object value)
        {
            variables[key] = value;
        }

        public object GetObject(string key)
        {
            variables.TryGetValue(key, out object value);
            return value;
        }

        // --- Utils ---
        public void Clear()
        {
            variables.Clear();
        }

        public bool HasKey(string key)
        {
            return variables.ContainsKey(key);
        }

        public void RemoveKey(string key)
        {
            if (variables.ContainsKey(key))
            {
                variables.Remove(key);
            }
        }
    }