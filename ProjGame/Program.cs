using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;
using System.IO;
class Program
{
    public static int shaderProgram;
    public static int shaderProgram1;
    public static int CompileShader(string source, ShaderType type)
    {
        // Создаём шейдер указанного типа (VertexShader или FragmentShader)
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source); // Загружаем исходный код шейдера
        GL.CompileShader(shader); // Компилируем шейдер

        // Проверяем статус компиляции
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
        if (status == 0) // Если произошла ошибка
        {
            string log = GL.GetShaderInfoLog(shader); // Получаем лог ошибки
            Console.WriteLine($"Ошибка компиляции {type}: {log}");
            throw new Exception("Не удалось скомпилировать шейдер."); // Генерируем исключение
        }

        return shader; // Возвращаем идентификатор шейдера
    }
    //шейдер для одноцветных текстур
    public static int CompileSaders()
    {
        string vertexShaderSource =
            @"#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aColor;

out vec3 FragColor;
out float alphaChannel;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float aChannel;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    FragColor = aColor;
    alphaChannel = aChannel;
}";

        string fragmentShaderSource =
            @"#version 330 core
in vec3 FragColor;
in float alphaChannel;

out vec4 FragColorOut;

void main()
{
    FragColorOut = vec4(FragColor, alphaChannel);

}";
        // Компиляция шейдеров
        int vertexShader = CompileShader(vertexShaderSource, ShaderType.VertexShader);
        int fragmentShader = CompileShader(fragmentShaderSource, ShaderType.FragmentShader);

        // Создание и связывание шейдерной программы
        int shader = GL.CreateProgram();
        GL.AttachShader(shader, vertexShader);
        GL.AttachShader(shader, fragmentShader);
        GL.LinkProgram(shader);

        Console.WriteLine($"Shader: {shader}");

        // Проверка на ошибки линковки
        GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out int linkStatus);
        if (linkStatus == 0)
        {
            string log = GL.GetProgramInfoLog(shader);
            Console.WriteLine($"Ошибка линковки программы: {log}");
            throw new Exception("Не удалось связать шейдерную программу.");
        }

        // Удаляем шейдеры после линковки
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        // Сохраняем ID программы для использования в рендеринге
        shaderProgram = shader;
        return shaderProgram;

    }
    //шейдер для текстур
    public static int CompileSaders1()
    {
        string vertexShaderSource =
            @"#version 330 core

            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec3 aColor;
            layout(location = 2) in vec2 aTexCoord;

            out vec3 FragColor;
            out vec2 TexCoord;
            out float alphaChannel;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;
            uniform float aChannel;

            void main()
            {
                gl_Position = projection * view * model * vec4(aPosition, 1.0);
                FragColor = aColor;
                TexCoord = aTexCoord;
                alphaChannel = aChannel;
            }
            ";

        string fragmentShaderSource =
            @"#version 330 core

            in vec3 FragColor;
            in vec2 TexCoord;
            in float alphaChannel;

            out vec4 FragColorOut;

            uniform sampler2D texture1;

            void main()
            {
                FragColorOut = texture(texture1, TexCoord);
            }
            ";
        // Компиляция шейдеров
        int vertexShader = CompileShader(vertexShaderSource, ShaderType.VertexShader);
        int fragmentShader = CompileShader(fragmentShaderSource, ShaderType.FragmentShader);

        // Создание и связывание шейдерной программы
        int shader = GL.CreateProgram();
        GL.AttachShader(shader, vertexShader);
        GL.AttachShader(shader, fragmentShader);
        GL.LinkProgram(shader);

        Console.WriteLine($"Shader: {shader}");

        // Проверка на ошибки линковки
        GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out int linkStatus);
        if (linkStatus == 0)
        {
            string log = GL.GetProgramInfoLog(shader);
            Console.WriteLine($"Ошибка линковки программы: {log}");
            throw new Exception("Не удалось связать шейдерную программу.");
        }

        // Удаляем шейдеры после линковки
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        // Сохраняем ID программы для использования в рендеринге
        shaderProgram1 = shader;
        return shaderProgram1;

    }


    public static int LoadTexture(string path)
    {
        int textureID = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureID);

        // Настройки фильтрации и обёртки
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(path))
        {
            image.Mutate(x => x.Flip(FlipMode.Vertical)); // OpenGL ожидает перевёрнутую по Y

            var pixels = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(pixels);

            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          image.Width,
                          image.Height,
                          0,
                          PixelFormat.Rgba,
                          PixelType.UnsignedByte,
                          pixels);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        return textureID;
    }


    static void Main()
    {

        int frameCount = 0;
        double timeElapsed = 0.0;
        double lastTime = 0.0;
        double fps = 0.0;

        int width = 1200;
        int height = 600;

        var settings = new NativeWindowSettings()
        {
            Size = new OpenTK.Mathematics.Vector2i(width, height),
            Title = "OpenGL with OpenTK 4.9.4",
        };
        GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
        gameWindowSettings.UpdateFrequency = 60.0;

        NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;

        using (var window = new GameWindow(gameWindowSettings, settings))
        {
            // === Настройка сцены ===

            // Камера
            Camera camera = new Camera(new Vector3(2.5f, 2.7f, -20.0f))
            {
                Yaw = 0.0f,
                Pitch = 0.0f
            };

            // Объекты
            Plane WorldPlane = new Plane(Xi: 0, Yi: 0, Zi: 0, sizeXi: 10000.0f, sizeZi: 10000.0f);
            WorldPlane.SetColorI(Ri: 93, Gi: 161, Bi: 48);

            Coquette car = new Coquette(0, -20);

            // Кубы
            var predefinedCubes = new List<Cube>
            {
                new Cube(0, 0, 0, 1, 1, 0, 0, 0),
                new Cube(-2, 0, 0, 1, 0, 1, 0, 45),
                new Cube(-4, 0, 0, 1, 1, 0, 0, 0),
                new Cube(-6, 0, 0, 1, 1, 0, 0, 0),
                new Cube(-8, 0, 0, 1, 1, 0, 0, 0),
                new Cube(-7, 0, 1.5f, 1, 1, 0, 0, 0),
                new Cube(-7, 0, -1.5f, 1, 1, 0, 0, 0),
                new Cube(-6, 0, -3, 1, 1, 0, 0, 0),
                new Cube(-6, 0, 3, 1, 1, 0, 0, 0),
            };
            predefinedCubes[0].SetColorI(139, 90, 43, 223, 198, 162); // cube1

            // Хранилища
            CubeStorage cubeStorage = new CubeStorage();
            PlaneStorage planeStorage = new PlaneStorage();

            cubeStorage.AddMultiple(predefinedCubes);

            // Деревья
            var treeList = new List<Tree>
            {
                new Oak(10, 0, 0),
                new BigOak(10, 0, 10),
                new Pine(10, 0, -10),
                new BigPine(10, 0, -20),
                new Brich(10, 0, 20),
                new SmallBrich(10, 0, 25),
                new Spruce(10, 0, -30),
                new BigSpruce(10, 0, -40)
            };

            foreach (var tree in treeList)
            {
                cubeStorage.AddMultiple(tree.GetCubes());
                planeStorage.AddMultiple(tree.GetPlanes());
            }

            // Генерация дороги
            RoadGenerator roadGen = new RoadGenerator();
            roadGen.GenerateLoopedRoad();

            List<Cube> roadCubes = roadGen.GetRoadCubes();
            cubeStorage.AddMultiple(roadCubes);

            // Деревья по всей местности
            TreePlacer treePlacer = new TreePlacer();
            List<Tree> placedTrees = treePlacer.PlaceTrees(200, 400, roadGen.RoadTiles);

            foreach (var tree in placedTrees)
            {
                cubeStorage.AddMultiple(tree.GetCubes());
                planeStorage.AddMultiple(tree.GetPlanes());
            }

            // === Uniform-локации (будут заполнены позже после компиляции шейдера) ===
            int viewLocation = 0;
            int projLocation = 0;
            int modelLocation = 0;
            int alphaLocation = 0;

            int viewLocation1 = 0;
            int projLocation1 = 0;
            int modelLocation1 = 0;
            int alphaLocation1 = 0;
            int texLocation1 = 0;
            int texID=0;
            float[] vertices =
                {
                    -5.0f, -5.5f, 1f, 1f,1f,1f,   0f, 0f,
                     5.0f, -5.5f, 1f, 1f,1f,1f,   1f, 0f,
                     5.0f,  5.5f, 1f, 1f,1f,1f, 1f, 1f,
                    -5.0f,  5.5f, 1f, 1f,1f,1f,  0f, 1f
                };
            uint[] indices123 = {
                    0, 1, 2,
                    2, 3, 0
                };

            // === Ввод мыши ===
            float mouseXOffset = 0;
            float mouseYOffset = 0;

            int vao=0, vbo=0, ebo=0;

            // === Настройка завершена ===
            window.Load += () =>
            {
                CompileSaders();
                shaderProgram1 = CompileSaders1();

                //текстурная плоскость

                GL.UseProgram(shaderProgram1);

                vao = GL.GenVertexArray();
                vbo = GL.GenBuffer();
                ebo = GL.GenBuffer();
                //Привязываем VAO
                GL.BindVertexArray(vao);

                //Привязываем VBO
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

                //Привязываем EBO
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices123.Length * sizeof(uint), indices123, BufferUsageHint.StaticDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
                GL.EnableVertexAttribArray(1);

                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));
                GL.EnableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);

                texID = LoadTexture(@"C:\\Users\\KODT\\Desktop\\Lab_work\\task1\\ConsoleApp1\\texture.jpg");

                if (texID == -1)
                {
                    Console.WriteLine("TexID err");
                }

                GL.UseProgram(shaderProgram);
                Matrix4 view = camera.GetViewMatrix();
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / (float)height, 0.1f, 150.0f);

                window.CursorState = CursorState.Grabbed;

                string version = GL.GetString(StringName.Version);
                Console.WriteLine($"Версия OpenGL: {version}");

                // Настройки OpenGL
                GL.ClearColor(0.4f, 0.5f, 0.8f, 1.0f); // Цвет фона
                GL.Enable(EnableCap.DepthTest); // Тест глубины для рендеринга 3D
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                WorldPlane.Load();

                car.Load();

                foreach (Cube cube in cubeStorage.allCubes)
                {
                    cube.Load();
                }
                foreach (Plane plane in planeStorage.allPlanes)
                {
                    plane.Load();
                }

            };

            window.RenderFrame += (FrameEventArgs args) =>
            {
                //Рассчет времени кадра
                double currentTime = GLFW.GetTime();
                timeElapsed += currentTime - lastTime;
                lastTime = currentTime;
                frameCount++;

                if (timeElapsed >= 1.0)
                {
                    fps = frameCount / timeElapsed;
                    Console.Write($"FPS: {fps:F1}");
                    frameCount = 0;
                    timeElapsed = 0.0;
                    Console.Write($"X: {car.X} Z: {car.Z}");
                }
                //Очистка экрана
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                Matrix4 view = camera.GetViewMatrix();
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), width / (float)height, 0.7f, 150.0f);


                viewLocation = GL.GetUniformLocation(shaderProgram, "view");
                projLocation = GL.GetUniformLocation(shaderProgram, "projection");
                modelLocation = GL.GetUniformLocation(shaderProgram, "model");
                alphaLocation = GL.GetUniformLocation(shaderProgram, "aChannel");

                viewLocation1 = GL.GetUniformLocation(shaderProgram1, "view");
                projLocation1 = GL.GetUniformLocation(shaderProgram1, "projection");
                modelLocation1 = GL.GetUniformLocation(shaderProgram1, "model");
                alphaLocation1 = GL.GetUniformLocation(shaderProgram1, "aChannel");
                texLocation1 = GL.GetUniformLocation(shaderProgram1, "texture1");




                GL.UniformMatrix4(projLocation, false, ref projection);
                GL.UniformMatrix4(viewLocation, false, ref view);

                WorldPlane.Draw(shaderProgram, modelLocation, alphaLocation);

                car.Draw(shaderProgram, modelLocation, alphaLocation);

                //текстурная плоскость
                GL.UseProgram(shaderProgram1);

                GL.BindVertexArray(vao);

                Matrix4 model = Matrix4.Identity;

                model *= Matrix4.CreateTranslation(new Vector3(0, 0, 0));

                if (texLocation1 == -1)
                    Console.WriteLine("Uniform 'texture1' not found!");
                GL.UniformMatrix4(modelLocation1, false, ref model);
                GL.UniformMatrix4(projLocation1, false, ref projection);
                GL.UniformMatrix4(viewLocation1, false, ref view);
                GL.Uniform1(alphaLocation1, 1.0);

                GL.ActiveTexture(TextureUnit.Texture0);
                // Привязываем нашу текстуру к этому юниту
                GL.BindTexture(TextureTarget.Texture2D, texID);
                GL.Uniform1(texLocation1, 0);

                GL.DrawElements(PrimitiveType.Triangles, indices123.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);





                foreach (Cube cube in cubeStorage.GetCubesNear(car.X, car.Z))
                {
                    cube.Draw(shaderProgram, modelLocation, alphaLocation);
                }
                foreach (Plane plane in planeStorage.GetPlanesNear(car.X, car.Z))
                {
                    plane.Draw(shaderProgram, modelLocation, alphaLocation);
                }

                GL.BindVertexArray(0);

                window.SwapBuffers();
            };
            window.Resize += (ResizeEventArgs args) =>
            {
                GL.Viewport(0, 0, window.Size.X, window.Size.Y);
            };
            window.UpdateFrame += (FrameEventArgs args) =>
            {
            float cameraSpeed = camera.Speed * (float)args.Time; // Скорость зависит от времени кадра

            car.Update((float)args.Time);

            var keyboardState = window.KeyboardState;

            float acceleration = 0.8f * (float)args.Time;
            float brakeDeceleration = 10.0f * (float)args.Time;
            float deceleration = 0.4f * (float)args.Time;
            float rotationAcceleration = 32.0f * (float)args.Time;
            float rotationDeceleration = 14.0f * (float)args.Time;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                car.Speed += acceleration;
                car.Speed = Math.Clamp(car.Speed, -20, 20);
            }
            else
            {
                car.Speed = MathF.Max(0, car.Speed - deceleration);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (car.Speed > 0)
                {
                    car.Speed -= brakeDeceleration;

                }
                else
                {
                    car.Speed -= acceleration;
                }
                car.Speed = Math.Clamp(car.Speed, -20, 20);
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                car.RotationSpeed += rotationAcceleration;
                car.RotationSpeed = Math.Clamp(car.RotationSpeed, -20, 20);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                car.RotationSpeed -= rotationAcceleration;
                car.RotationSpeed = Math.Clamp(car.RotationSpeed, -20, 20);
            }
            else
            {
                if (car.RotationSpeed > 0)
                    car.RotationSpeed = MathF.Max(0, car.RotationSpeed - rotationDeceleration);
                else if (car.RotationSpeed < 0)
                    car.RotationSpeed = MathF.Min(0, car.RotationSpeed + rotationDeceleration);
            }

            if (window.KeyboardState.IsKeyDown(Keys.Escape))
            {
                window.Close();
            }

            camera.Yaw += mouseXOffset;
            camera.Pitch -= mouseYOffset;

            camera.Pitch = Math.Clamp(camera.Pitch, -3.0f, 89.0f);


            float camDistance = 12.0f;

            Vector3 target = new Vector3(car.X, 1.0f, car.Z);

            float yawRad = MathHelper.DegreesToRadians(camera.Yaw);
            float pitchRad = MathHelper.DegreesToRadians(camera.Pitch);

            Vector3 cameraOffset = new Vector3
            {
                X = camDistance * (float)(Math.Cos(pitchRad) * Math.Cos(yawRad)),
                Y = camDistance * (float)(Math.Sin(pitchRad)),
                Z = camDistance * (float)(Math.Cos(pitchRad) * Math.Sin(yawRad))
            };

            camera.Position = target + cameraOffset;

            camera.Front = Vector3.Normalize(target - camera.Position);

            mouseXOffset = 0;
            mouseYOffset = 0;

        };
            window.MouseMove += (MouseMoveEventArgs e) =>
            {
                mouseXOffset = e.DeltaX * camera.Sensitivity;
                mouseYOffset = e.DeltaY * camera.Sensitivity;

            };

            window.Run();
        }
    }
}