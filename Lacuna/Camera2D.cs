using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lacuna {
    public class Camera2D {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public Matrix Transform { get; protected set; }

        private float currentMouseWheelValue;
        float previousMouseWheelValue;
        float zoom;
        float previousZoom;

        // ------------------------------------------------------------------------------------------
        public Camera2D(Viewport viewport) {
            Bounds = viewport.Bounds;
            Zoom = 1f;
            Position = Vector2.Zero;
        }

        // ------------------------------------------------------------------------------------------
        private void UpdateVisibleArea() {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        // ------------------------------------------------------------------------------------------
        private void UpdateMatrix() {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        // ------------------------------------------------------------------------------------------
        public void MoveCamera(Vector2 movePosition) {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }

        // ------------------------------------------------------------------------------------------
        public void AdjustZoom(float zoomAmount) {
            Zoom += zoomAmount;
            if (Zoom < 1.0f) {
                Zoom = 1.0f;
            }
            else if (Zoom > 5.0f) {
                Zoom = 5.0f;
            }
        }

        // ------------------------------------------------------------------------------------------
        public void Update(Viewport bounds, GameTime gameTime, KeyboardState NewKeyState, KeyboardState OldKeyState, MouseState mouseState) {
            Bounds = bounds.Bounds;
            UpdateMatrix();

            Vector2 cameraMovement = Vector2.Zero;
            int moveSpeed;

            if (Zoom > .8f) {
                moveSpeed = 150;
            }
            else if (Zoom < .8f && Zoom >= .6f) {
                moveSpeed = 20;
            }
            else if (Zoom < .6f && Zoom > .35f) {
                moveSpeed = 25;
            }
            else if (Zoom <= .35f) {
                moveSpeed = 30;
            }
            else {
                moveSpeed = 10;
            }

            // Keyboard
            if (NewKeyState.IsKeyDown(Keys.Up)) {
                cameraMovement.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (NewKeyState.IsKeyDown(Keys.Down)) {
                cameraMovement.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (NewKeyState.IsKeyDown(Keys.Left)) {
                cameraMovement.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (NewKeyState.IsKeyDown(Keys.Right)) {
                cameraMovement.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Mouse
            if (mouseState.Position.Y <= 0) {
                cameraMovement.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (mouseState.Position.Y >= Bounds.Height - 1) {
                cameraMovement.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (mouseState.Position.X <= 0) {
                cameraMovement.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (mouseState.Position.X >= Bounds.Width - 1) {
                cameraMovement.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = mouseState.ScrollWheelValue;

            if (currentMouseWheelValue > previousMouseWheelValue) {
                AdjustZoom(1.0f);
            }

            if (currentMouseWheelValue < previousMouseWheelValue) {
                AdjustZoom(-1.0f);
            }

            previousZoom = zoom;
            zoom = Zoom;
            if (previousZoom != zoom) {

            }

            cameraMovement.X = (float)Math.Round(cameraMovement.X);
            cameraMovement.Y = (float)Math.Round(cameraMovement.Y);
            MoveCamera(cameraMovement);
        }
    }
}