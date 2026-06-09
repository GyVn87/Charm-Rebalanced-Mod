using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TuyenTuyenTuyen.Mechanics {
    internal static class Utilities {
        internal static readonly FieldInfo stunControlFSM = typeof(HealthManager).GetField("stunControlFSM", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static float GetAngleBetween2Object(GameObject GO1, GameObject GO2) {
            float num = GO2.transform.position.y - GO1.transform.position.y;
            float num2 = GO2.transform.position.x - GO1.transform.position.x;
            float num3;
            for (num3 = Mathf.Atan2(num, num2) * (180f / (float)Math.PI); num3 < 0f; num3 += 360f) { }
            return num3;
        }

        internal static void SetVelocityAsAngle(GameObject gameObject, float angle, float speed) {
            Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
            if (rb2d == null)
                return;
            float x = speed * Mathf.Cos(angle * ((float)Math.PI / 180f));
            float y = speed * Mathf.Sin(angle * ((float)Math.PI / 180f));
            Vector2 newVelocity = default;
            newVelocity.x = x;
            newVelocity.y = y;
            rb2d.velocity = newVelocity;
        }

        internal static Sprite LoadSprite(string spriteName) {
            Assembly asm = Assembly.GetExecutingAssembly();
            Sprite charmSprite;
            using (Stream stream = asm.GetManifestResourceStream($"TuyenTuyenTuyen.Assets.Charms.{spriteName}.png")) {
                if (stream == null) return null;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Texture2D texture = new Texture2D(1, 1);
                ImageConversion.LoadImage(texture, buffer);
                charmSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            return charmSprite;
        }
    }
}
