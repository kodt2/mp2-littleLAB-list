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

public class Object
{
    public static uint[] indicesCube = new uint[]
    {
    0, 1, 2, 2, 3, 0,    // Передняя грань
    4, 5, 6, 6, 7, 4,    // Задняя грань
    8, 9, 10, 10, 11, 8,  // Левая грань
    12, 13, 14, 14, 15, 12, // Правая грань
    16, 17, 18, 18, 19, 16, // Верхняя грань
    20, 21, 22, 22, 23, 20  // Нижняя грань
    };
    public static uint[] indicesPlane = new uint[]
    {
    0, 2, 1, 1, 3, 0};
}
public class Cube : Object
{
    public float X { get; set; }   // Координата центра по оси X
    public float Y { get; set; }   // Координата центра по оси Y
    public float Z { get; set; }   // Координата центра по оси Z
    public float Size { get; set; } // Сторона куба
    public float RotationX { get; set; } // Поворот по оси X (в градусах)
    public float RotationY { get; set; } // Поворот по оси Y (в градусах)
    public float RotationZ { get; set; } // Поворот по оси Z (в градусах)
    public float Angle { get; set; }
    public int VAO;
    public int VBO;
    public int EBO;
    private float[] ColorVertices;



    public Cube()
    {
        X = 0;
        Y = 0;
        Z = 0;
        Size = 1 - 0.0002f / 2;
        RotationX = 0;
        RotationY = 0;
        RotationZ = 0;
        ColorVertices = new float[6];
        for (int indexer = 0; indexer < 6; indexer++)
        {
            ColorVertices[indexer] = 1f;
        }
    }
    public Cube(float x, float y, float z, float size, float KrotationX, float KrotationY, float KrotationZ, float angle)
    {
        X = x;
        Y = y;
        Z = z;
        Size = size - 0.0002f / 2;
        RotationX = KrotationX;
        RotationY = KrotationY;
        RotationZ = KrotationZ;
        Angle = angle;
        ColorVertices = new float[6];
        for (int indexer = 0; indexer < 6; indexer++)
        {
            ColorVertices[indexer] = 1.0f;
        }
    }

    public void SetColorR(float mainR, float mainG, float mainB, float altR, float altG, float altB)
    {
        ColorVertices[0] = mainR - 0.35f;
        ColorVertices[1] = mainG - 0.35f;
        ColorVertices[2] = mainB - 0.35f;
        ColorVertices[3] = altR - 0.35f;
        ColorVertices[4] = altG - 0.35f;
        ColorVertices[5] = altB - 0.35f;
    }
    public void SetColorI(int mainR, int mainG, int mainB, int altR, int altG, int altB)
    {
        ColorVertices[0] = mainR / 255.0f;
        ColorVertices[1] = mainG / 255.0f;
        ColorVertices[2] = mainB / 255.0f;
        ColorVertices[3] = altR / 255.0f;
        ColorVertices[4] = altG / 255.0f;
        ColorVertices[5] = altB / 255.0f;
    }

    // Метод для расчета координат 8 вершин куба
    public float[] GetCoordinates()
    {
        // Половина стороны куба (для удобства вычислений)
        float half = Size / 2;

        // 8 вершин куба относительно его центра
        float[] vertices = new float[]
        {
        // Позиция              // Цвет               
        // Передняя грань
        -half, -half,  half,     ColorVertices[0]+0.05f+(Y-1)*0.006f, ColorVertices[1]+0.05f+(Y-1)*0.006f, ColorVertices[2]+0.05f+(Y-1)*0.006f,
         half, -half,  half,     ColorVertices[0]+0.05f+(Y-1)*0.006f, ColorVertices[1]+0.05f+(Y-1)*0.006f, ColorVertices[2]+0.05f+(Y-1)*0.006f,
         half,  half,  half,     ColorVertices[0]+0.05f+Y*0.006f, ColorVertices[1]+0.05f+Y*0.006f, ColorVertices[2]+0.05f+Y*0.006f,
        -half,  half,  half,     ColorVertices[0]+0.05f+Y*0.006f, ColorVertices[1]+0.05f+Y*0.006f, ColorVertices[2]+0.05f+Y*0.006f,

        // Задняя грань
        -half, -half, -half,     ColorVertices[0]-0.05f+(Y-1)*0.006f, ColorVertices[1]-0.05f+(Y-1)*0.006f, ColorVertices[2]-0.05f+(Y-1)*0.006f,
         half, -half, -half,     ColorVertices[0]-0.05f+(Y-1)*0.006f, ColorVertices[1]-0.05f+(Y-1)*0.006f, ColorVertices[2]-0.05f+(Y-1)*0.006f,
         half,  half, -half,     ColorVertices[0]-0.05f+Y*0.006f, ColorVertices[1]-0.05f+Y*0.006f, ColorVertices[2]-0.05f+Y*0.006f,
        -half,  half, -half,     ColorVertices[0]-0.05f+Y*0.006f, ColorVertices[1]-0.05f+Y*0.006f, ColorVertices[2]-0.05f+Y*0.006f,

        // Левая грань
        -half, -half, -half,     ColorVertices[0]+0.025f+(Y-1)*0.006f, ColorVertices[1]+0.025f+(Y-1)*0.006f, ColorVertices[2]+0.025f+(Y-1)*0.006f,
        -half, -half,  half,     ColorVertices[0]+0.025f+(Y-1)*0.006f, ColorVertices[1]+0.025f+(Y-1)*0.006f, ColorVertices[2]+0.025f+(Y-1)*0.006f,
        -half,  half,  half,     ColorVertices[0]+0.025f+Y*0.006f, ColorVertices[1]+0.025f+Y*0.006f, ColorVertices[2]+0.025f+Y*0.006f,
        -half,  half, -half,     ColorVertices[0]+0.025f+Y*0.006f, ColorVertices[1]+0.025f+Y*0.006f, ColorVertices[2]+0.025f+Y*0.006f,

        // Правая грань
         half, -half, -half,     ColorVertices[0]-0.025f+(Y-1)*0.006f, ColorVertices[1]-0.025f+(Y-1)*0.006f, ColorVertices[2]-0.025f+(Y-1)*0.006f,
         half, -half,  half,     ColorVertices[0]-0.025f+(Y-1)*0.006f, ColorVertices[1]-0.025f+(Y-1)*0.006f, ColorVertices[2]-0.025f+(Y-1)*0.006f,
         half,  half,  half,     ColorVertices[0]-0.025f+Y*0.006f, ColorVertices[1]-0.025f+Y*0.006f, ColorVertices[2]-0.025f+Y*0.006f,
         half,  half, -half,     ColorVertices[0]-0.025f+Y*0.006f, ColorVertices[1]-0.025f+Y*0.006f, ColorVertices[2]-0.025f+Y*0.006f,

        // Верхняя грань
        -half,  half,  half,     ColorVertices[3]+0.1f, ColorVertices[4]+0.1f, ColorVertices[5]+0.1f,
         half,  half,  half,     ColorVertices[3]+0.1f, ColorVertices[4]+0.1f, ColorVertices[5]+0.1f,
         half,  half, -half,     ColorVertices[3]+0.1f, ColorVertices[4]+0.1f, ColorVertices[5]+0.1f,
        -half,  half, -half,     ColorVertices[3]+0.1f, ColorVertices[4]+0.1f, ColorVertices[5]+0.1f,

        // Нижняя грань
        -half, -half,  half,     ColorVertices[3]-0.1f, ColorVertices[4]-0.1f, ColorVertices[5] - 0.1f,
         half, -half,  half,     ColorVertices[3]-0.1f, ColorVertices[4]-0.1f, ColorVertices[5]-0.1f,
         half, -half, -half,     ColorVertices[3]-0.1f, ColorVertices[4]-0.1f, ColorVertices[5]-0.1f,
        -half, -half, -half,     ColorVertices[3]-0.1f, ColorVertices[4]-0.1f, ColorVertices[5]-0.1f,
        };

        return vertices;

    }

    public void Load()
    {
        float[] data = GetCoordinates();

        //Создаем
        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();
        //Привязываем VAO
        GL.BindVertexArray(VAO);

        //Привязываем VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

        //Привязываем EBO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indicesCube.Length * sizeof(uint), indicesCube, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
        GL.EnableVertexAttribArray(1);


        //GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), (IntPtr)(6 * sizeof(float)));
        //GL.EnableVertexAttribArray(2);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        GL.UseProgram(shaderProgram);

        GL.BindVertexArray(VAO);

        Matrix4 model = Matrix4.Identity;
        if (Angle == 0)
        {
            RotationX = 1;
        }
        model *= Matrix4.CreateFromAxisAngle(new Vector3(RotationX, RotationY, RotationZ).Normalized(), MathHelper.DegreesToRadians(Angle));
        model *= Matrix4.CreateTranslation(new Vector3(X, Y + 0.5f * Size + 0.0002f, Z));

        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.Uniform1(alphaLocation, 1f);

        GL.DrawElements(PrimitiveType.Triangles, indicesCube.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
    public override bool Equals(object? obj)
    {
        if (obj is not Cube other)
            return false;

        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
public class Plane : Object
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float sizeX { get; set; }
    public float sizeZ { get; set; }
    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public int VAO { get; set; }
    public int VBO { get; set; }
    public int EBO { get; set; }
    public float alpha = 1.0f;



    public Plane(float Xi, float Yi, float Zi, float sizeXi, float sizeZi)
    {
        X = Xi;
        Y = Yi;
        Z = Zi;
        sizeX = sizeXi;
        sizeZ = sizeZi;
        R = 1;
        G = 1; B = 1;
        alpha = 1.0f;
    }
    public void SetColorF(float Ri, float Gi, float Bi)
    {
        R = Ri;
        G = Gi;
        B = Bi;
    }
    public void SetColorI(int Ri, int Gi, int Bi)
    {
        R = Ri / 255.0f;
        G = Gi / 255.0f;
        B = Bi / 255.0f;
    }
    public void SetAlpha(float a)
    {
        alpha = a;
    }
    public void Load()
    {

        float[] data = new float[]
        {
            sizeX/2, Y, sizeZ/2, R, G,B,
            -sizeX/2, Y, -sizeZ/2, R, G,B,
            -sizeX/2, Y, sizeZ/2, R, G,B,
            sizeX/2, Y, -sizeZ/2, R, G,B
        };
        //Создаем
        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();
        //Привязываем VAO
        GL.BindVertexArray(VAO);

        //Привязываем VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

        //Привязываем EBO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indicesPlane.Length * sizeof(uint), indicesPlane, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
        GL.EnableVertexAttribArray(1);

        //GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), (IntPtr)(6 * sizeof(float)));
        //GL.EnableVertexAttribArray(2);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

    }
    public void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        GL.UseProgram(shaderProgram);

        GL.BindVertexArray(VAO);

        Matrix4 model = Matrix4.Identity;

        model *= Matrix4.CreateTranslation(new Vector3(X, Y, Z));

        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.Uniform1(alphaLocation, alpha);

        GL.DrawElements(PrimitiveType.Triangles, indicesPlane.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
}
public class Tree : Object
{
    public float X = 0;
    public float Y = 0;
    public float Z = 0;
    public List<Cube> Trunk = new();
    public virtual List<Cube> Leaves { get; } = new();
    public List<Plane> Shadow = new();

    public Tree() { }

    public Tree(float xi, float yi, float zi, int height, int trunkDiam,
        int mainTrunkR, int mainTrunkG, int mainTrunkB,
        int altTrunkR, int altTrunkG, int altTrunkB)
    {
        X = xi;
        Y = yi;
        Z = zi;

        GenerateShadows(trunkDiam);
        GenerateTrunk(height, trunkDiam, mainTrunkR, mainTrunkG, mainTrunkB, altTrunkR, altTrunkG, altTrunkB);
    }

    private void GenerateShadows(int trunkDiam)
    {
        var shadowData = new List<(float dx, float dz, float alpha)>();

        if (trunkDiam == 1)
        {
            shadowData = new()
            {
                (-1, 0, 0.1f), (-1, -1, 0.05f), (0, -1, 0.1f), (1, -1, 0.05f),
                (1, 0, 0.1f), (1, 1, 0.05f), (0, 1, 0.1f), (-1, 1, 0.05f),
                (-2, 0, 0.02f), (2, 0, 0.02f), (0, 2, 0.02f), (0, -2, 0.02f)
            };
        }
        else if (trunkDiam == 2)
        {
            shadowData = new()
            {
                (-1, 0, 0.15f), (-1, -1, 0.1f), (0, -1, 0.15f), (1, -1, 0.15f),
                (2, -1, 0.1f), (2, 0, 0.15f), (2, 1, 0.15f), (2, 2, 0.1f),
                (1, 2, 0.15f), (0, 2, 0.15f), (-1, 2, 0.1f), (-1, 1, 0.15f),
                (0, -2, 0.05f), (1, -2, 0.05f), (3, 0, 0.05f), (3, 1, 0.05f),
                (0, 3, 0.05f), (1, 3, 0.05f), (-2, 1, 0.05f), (-2, 0, 0.05f)
            };
        }

        foreach (var (dx, dz, alpha) in shadowData)
        {
            var plane = new Plane(X + dx, Y + 0.008f, Z + dz, 1, 1);
            plane.SetColorI(0, 0, 0);
            plane.SetAlpha(alpha);
            Shadow.Add(plane);
        }
    }

    private void GenerateTrunk(int height, int trunkDiam,
        int mainTrunkR, int mainTrunkG, int mainTrunkB,
        int altTrunkR, int altTrunkG, int altTrunkB)
    {
        int offset = trunkDiam % 2 == 0 ? 0 : -trunkDiam / 2;

        for (int k = 0; k < trunkDiam; k++)
        {
            for (int j = 0; j < trunkDiam; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    float trunkX = X + k + offset;
                    float trunkZ = Z + j + offset;
                    float trunkY = Y + i;
                    var cube = new Cube(trunkX, trunkY, trunkZ, 1.0f, 0, 0, 0, 0);
                    cube.SetColorI(mainTrunkR, mainTrunkG, mainTrunkB, altTrunkR, altTrunkG, altTrunkB);
                    Trunk.Add(cube);
                }
            }
        }
    }

    public List<Cube> GetCubes()
    {
        return Leaves.Concat(Trunk).ToList();
    }
    public List<Plane> GetPlanes()
    {
        return Shadow;
    }
}
public class Oak : Tree
{
    public override List<Cube> Leaves { get; } = new();

    public Oak(float x, float y, float z) : base(x, y, z, 5, 1, 139, 90, 43, 223, 198, 162)
    {
        int k = 0;
        for (int ix = -2; ix <= 2; ix++)
        {
            for (int iy = 2; iy <= 6; iy++)
            {
                for (int iz = -2; iz <= 2; iz++)
                {
                    if ((ix == 2 || ix == -2) &&
                        (iz == 2 || iz == -2) && iy < 5)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 5)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 6)
                    { }
                    else if ((ix == 1 || ix == -1) &&
                        (iz == 1 || iz == -1) && iy == 6)
                    { }
                    else
                    {
                        Cube c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(60, 138, 37, 60, 138, 37);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }
}
public class BigOak : Tree
{
    public override List<Cube> Leaves { get; } = new();

    public BigOak(float x, float y, float z) : base(x, y, z, 10, 2, 139, 90, 43, 223, 198, 162)
    {
        int k = 0;
        for (int ix = -3; ix <= 4; ix++)
        {
            for (int iy = 6; iy <= 12; iy++)
            {
                for (int iz = -3; iz <= 4; iz++)
                {
                    if ((ix == 4 || ix == -3) &&
                        (iz == 4 || iz == -3) && iy < 11)
                    { }
                    else if (((ix == 4 || ix == -3) ||
                        (iz == 4 || iz == -3)) && iy == 11)
                    { }
                    else if (((ix == 4 || ix == -3) ||
                        (iz == 4 || iz == -3)) && iy == 12)
                    { }
                    else if ((ix == 3 || ix == -2) &&
                        (iz == 3 || iz == -2) && iy == 12)
                    { }
                    else
                    {
                        Cube c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(60, 138, 37, 60, 138, 37);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }
}
public class Pine : Tree
{

    public override List<Cube> Leaves { get; } = new();

    public Pine(float x, float y, float z) : base(x, y, z, 7, 1, 170, 108, 57, 222, 184, 135)
    {
        for (int ix = -3; ix <= 3; ix++)
        {
            for (int iy = 6; iy <= 9; iy++)
            {
                for (int iz = -3; iz <= 3; iz++)
                {
                    if ((ix == 3 || ix == -3) &&
                        (iz == 3 || iz == -3) && iy < 8)
                    { }
                    else if (((ix == 3 || ix == -3) ||
                        (iz == 3 || iz == -3)) && iy == 8)
                    { }
                    else if (((ix == 3 || ix == -3) ||
                        (iz == 3 || iz == -3)) && iy == 9)
                    { }
                    else if ((ix == 2 || ix == -2) &&
                        (iz == 2 || iz == -2) && iy == 9)
                    { }
                    else
                    {
                        Cube c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(67, 105, 85, 67, 105, 85);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }
}
public class BigPine : Tree
{

    public override List<Cube> Leaves { get; } = new();

    public BigPine(float x, float y, float z) : base(x, y, z, 14, 2, 170, 108, 57, 222, 184, 135)
    {
        for (int ix = -4; ix <= 5; ix++)
        {
            for (int iy = 11; iy <= 14; iy++)
            {
                for (int iz = -4; iz <= 5; iz++)
                {
                    if ((ix == 5 || ix == -4) &&
                        (iz == 5 || iz == -4) && iy < 13)
                    { }
                    else if (((ix == 5 || ix == -4) ||
                        (iz == 5 || iz == -4)) && iy == 13)
                    { }
                    else if (((ix == 5 || ix == -4) ||
                        (iz == 5 || iz == -4)) && iy == 14)
                    { }
                    else if ((ix >= 3 || ix <= -2) &&
                        (iz >= 3 || iz <= -2) && iy == 14)
                    { }
                    else
                    {
                        Cube c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(67, 105, 85, 67, 105, 85);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }
}
public class Brich : Tree
{
    public override List<Cube> Leaves { get; } = new();

    public Brich(float x, float y, float z) : base(x, y, z, 5, 1, 230, 220, 220, 245, 222, 179)
    {
        for (int ix = -2; ix <= 2; ix++)
        {
            for (int iy = 2; iy <= 6; iy++)
            {
                for (int iz = -2; iz <= 2; iz++)
                {
                    if ((ix == 2 || ix == -2) &&
                        (iz == 2 || iz == -2) && iy < 5)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 5)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 6)
                    { }
                    else if ((ix == 1 || ix == -1) &&
                        (iz == 1 || iz == -1) && iy == 6)
                    { }
                    else
                    {
                        var c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(100, 168, 80, 100, 168, 80);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }
}
public class SmallBrich : Tree
{
    public override List<Cube> Leaves { get; } = new();

    public SmallBrich(float x, float y, float z) : base(x, y, z, 3, 1, 230, 220, 220, 245, 222, 179)
    {

        var c0 = new Cube(x + 1, y + 1, z + 0, 1, 0, 0, 0, 0);
        c0.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c0);
        var c1 = new Cube(x + 1, y + 2, z + 0, 1, 0, 0, 0, 0);
        c1.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c1);
        var c2 = new Cube(x + 0, y + 1, z + 1, 1, 0, 0, 0, 0);
        c2.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c2);
        var c3 = new Cube(x + 0, y + 2, z + 1, 1, 0, 0, 0, 0);
        c3.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c3);
        var c4 = new Cube(x - 1, y + 1, z + 0, 1, 0, 0, 0, 0);
        c4.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c4);
        var c5 = new Cube(x - 1, y + 2, z + 0, 1, 0, 0, 0, 0);
        c5.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c5);
        var c6 = new Cube(x + 0, y + 1, z + -1, 1, 0, 0, 0, 0);
        c6.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c6);
        var c7 = new Cube(x + 0, y + 2, z + -1, 1, 0, 0, 0, 0);
        c7.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c7);
        var c8 = new Cube(x + 0, y + 3, z + 0, 1, 0, 0, 0, 0);
        c8.SetColorI(100, 168, 80, 100, 168, 80);
        Leaves.Add(c8);
    }
}
public class Spruce : Tree
{

    public override List<Cube> Leaves { get; } = new();

    public Spruce(float x, float y, float z) : base(x, y, z, 6, 1, 110, 85, 65, 205, 170, 125)
    {
        int k = 0;
        for (int ix = -2; ix <= 2; ix++)
        {
            for (int iy = 1; iy <= 8; iy++)
            {
                for (int iz = -2; iz <= 2; iz++)
                {
                    if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 1)
                    { }
                    else if ((ix == 1 || ix == -1) &
                        (iz == 1 || iz == -1) && iy == 1)
                    { }
                    else if ((ix == 2 || ix == -2) &&
                        (iz == 2 || iz == -2) && iy == 2)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 3)
                    { }
                    else if (iy == 4) { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 5)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 6)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 7)
                    { }
                    else if ((ix == 1 || ix == -1) &&
                        (iz == 1 || iz == -1) && iy == 7)
                    { }
                    else if (((ix == 2 || ix == -2) ||
                        (iz == 2 || iz == -2)) && iy == 8)
                    { }
                    else if ((ix == 1 || ix == -1) &&
                        (iz == 1 || iz == -1) && iy == 8)
                    { }
                    else
                    {
                        var c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(52, 89, 70, 52, 89, 70);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }

}
public class BigSpruce : Tree
{
    public override List<Cube> Leaves { get; } = new();

    public BigSpruce(float x, float y, float z) : base(x, y, z, 14, 2, 110, 85, 65, 205, 170, 125)
    {
        int k = 0;
        for (int ix = -3; ix <= 4; ix++)
        {
            for (int iy = 2; iy <= 14; iy++)
            {
                for (int iz = -3; iz <= 4; iz++)
                {
                    if ((
                        (ix == -3 && iz == 4) || (ix == -3 && iz == 3) ||
                        (ix == -2 && iz == 4) || (ix == 4 && iz == 4) ||
                        (ix == 4 && iz == 3) || (ix == 3 && iz == 4) ||
                        (ix == 4 && iz == -3) || (ix == 4 && iz == -2) ||
                        (ix == 3 && iz == -3) || (ix == -3 && iz == -3) ||
                        (ix == -3 && iz == -2) || (ix == -2 && iz == -3)) && (iy == 2 || iy == 3)) continue;
                    if ((ix < -2 || ix > 3 || iz < -2 || iz > 3) && (iy == 4 || iy == 5))
                    {
                        continue;
                    }
                    if ((
                        (ix == -2 && iz == 3) || (ix == -2 && iz == 2) ||
                        (ix == -1 && iz == 3) || (ix == 3 && iz == 3) ||
                        (ix == 3 && iz == 2) || (ix == 2 && iz == 3) ||
                        (ix == 3 && iz == -2) || (ix == 3 && iz == -1) ||
                        (ix == 2 && iz == -2) || (ix == -2 && iz == -2) ||
                        (ix == -2 && iz == -1) || (ix == -1 && iz == -2)) && (iy == 4 || iy == 5)) continue;
                    if ((
                        (ix == -3 && iz == 4) || (ix == -3 && iz == 3) ||
                        (ix == -2 && iz == 4) || (ix == 4 && iz == 4) ||
                        (ix == 4 && iz == 3) || (ix == 3 && iz == 4) ||
                        (ix == 4 && iz == -3) || (ix == 4 && iz == -2) ||
                        (ix == 3 && iz == -3) || (ix == -3 && iz == -3) ||
                        (ix == -3 && iz == -2) || (ix == -2 && iz == -3)) && (iy == 6)) continue;
                    if ((ix < -2 || ix > 3 || iz < -2 || iz > 3) && (iy == 7))
                    {
                        continue;
                    }
                    if ((
                        (ix == -2 && iz == 3) || (ix == -2 && iz == 2) ||
                        (ix == -1 && iz == 3) || (ix == 3 && iz == 3) ||
                        (ix == 3 && iz == 2) || (ix == 2 && iz == 3) ||
                        (ix == 3 && iz == -2) || (ix == 3 && iz == -1) ||
                        (ix == 2 && iz == -2) || (ix == -2 && iz == -2) ||
                        (ix == -2 && iz == -1) || (ix == -1 && iz == -2)) && (iy == 7)) continue;
                    if ((ix < -1 || ix > 2 || iz < -1 || iz > 2) && (iy == 8 || iy == 9))
                    {
                        continue;
                    }
                    if (((ix == -1 && iz == 2) || (ix == 2 && iz == 2) ||
                        (ix == 2 && iz == -1) || (ix == -1 && iz == -1)) && (iy == 8 || iy == 9)) continue;
                    if ((ix < -2 || ix > 3 || iz < -2 || iz > 3) && (iy == 10))
                    {
                        continue;
                    }
                    if ((
                        (ix == -2 && iz == 3) || (ix == -2 && iz == 2) ||
                        (ix == -1 && iz == 3) || (ix == 3 && iz == 3) ||
                        (ix == 3 && iz == 2) || (ix == 2 && iz == 3) ||
                        (ix == 3 && iz == -2) || (ix == 3 && iz == -1) ||
                        (ix == 2 && iz == -2) || (ix == -2 && iz == -2) ||
                        (ix == -2 && iz == -1) || (ix == -1 && iz == -2)) && (iy == 10)) continue;
                    if ((ix < -1 || ix > 2 || iz < -1 || iz > 2) && (iy == 11))
                    {
                        continue;
                    }
                    if (((ix == -1 && iz == 2) || (ix == 2 && iz == 2) ||
                        (ix == 2 && iz == -1) || (ix == -1 && iz == -1)) && (iy == 11)) continue;
                    if (iy == 12) continue;
                    if ((ix < -1 || ix > 2 || iz < -1 || iz > 2) && (iy == 13))
                    {
                        continue;
                    }
                    if (((ix == -1 && iz == 2) || (ix == 2 && iz == 2) ||
                        (ix == 2 && iz == -1) || (ix == -1 && iz == -1)) && (iy == 13)) continue;
                    if ((ix < 0 || ix > 1 || iz < 0 || iz > 1) && (iy == 14))
                    {
                        continue;
                    }
                    else
                    {
                        var c = new Cube(x + ix, y + iy, z + iz, 1, 0, 0, 0, 0);
                        c.SetColorI(52, 89, 70, 52, 89, 70);
                        Leaves.Add(c);
                    }
                }
            }
        }
    }

}
public class Monkey : Plane
{
    float X = 0;
    float Y = 0;
    float Z = 0;
    bool rl = false;
    public Monkey(float x, float y, float z, bool uj) : base(x, y, z, 1, 1)
    {
        X = x;
        Y = y;
        Z = z;
        rl = uj;
    }
    public new void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        GL.UseProgram(shaderProgram);

        GL.BindVertexArray(VAO);

        Matrix4 model = Matrix4.Identity;
        // Сначала применяем трансляцию, чтобы переместить объект в нужную позицию
        Matrix4 translationMatrix = Matrix4.CreateTranslation(new Vector3(X, Y - 0.4f, Z));

        // Затем применяем повороты, чтобы вращать объект в нужном направлении
        Matrix4 rotationMatrixX;
        Matrix4 rotationMatrixY;

        // Перемещаем объект, затем применяем повороты

        if (rl)
        {
            rotationMatrixX = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0).Normalized(), MathHelper.DegreesToRadians(90));
            rotationMatrixY = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1).Normalized(), MathHelper.DegreesToRadians(45));

        }
        else
        {
            rotationMatrixX = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0).Normalized(), MathHelper.DegreesToRadians(90));
            rotationMatrixY = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1).Normalized(), MathHelper.DegreesToRadians(-45));
        }

        model = rotationMatrixY * rotationMatrixX * translationMatrix;
        //model = rotationMatrixX * translationMatrix;


        GL.UniformMatrix4(modelLocation, false, ref model);
        GL.Uniform1(alphaLocation, alpha);

        GL.DrawElements(PrimitiveType.Triangles, indicesPlane.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }
}
public class Grass : Object
{
    public float X;
    public float Y;
    public float Z;
    public Monkey[] Gras { get; set; }
    public Grass(float x, float z)
    {
        Gras = new Monkey[2];
        X = x;
        Y = 0;
        Z = z;
        Gras[0] = new Monkey(X, Y, Z, false);
        Gras[0].SetColorI(121, 191, 73);
        Gras[1] = new Monkey(X, Y, Z, true);
        Gras[1].SetColorI(121, 191, 73);
    }
    public void Load()
    {
        Gras[0].Load();
        Gras[1].Load();
    }

    public void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        Gras[0].Draw(shaderProgram, modelLocation, alphaLocation);
        Gras[1].Draw(shaderProgram, modelLocation, alphaLocation);
    }
}
public class GrassField
{
    float X;
    float Z;
    Grass g;
    public static uint[] indicesPlane = new uint[]
    {
    0, 2, 1, 1, 3, 0};
    public GrassField(float x, float z)
    {
        X = x;
        Z = z;
        g = new Grass(x, z);
    }
    public void Update(float x, float z)
    {
        X = x;
        Z = z;
    }
    public void Load()
    {
        g.Load();
    }
    public void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        GL.UseProgram(shaderProgram);

        GL.BindVertexArray(g.Gras[0].VAO);

        Matrix4 model = Matrix4.Identity;
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                // Сначала применяем трансляцию, чтобы переместить объект в нужную позицию
                Matrix4 translationMatrix = Matrix4.CreateTranslation(new Vector3(((int)X + i), -0.4f, ((int)Z + j)));

                // Затем применяем повороты, чтобы вращать объект в нужном направлении
                Matrix4 rotationMatrixX;
                Matrix4 rotationMatrixY;

                rotationMatrixX = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0).Normalized(), MathHelper.DegreesToRadians(90));
                rotationMatrixY = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1).Normalized(), MathHelper.DegreesToRadians(45));

                model = rotationMatrixY * rotationMatrixX * translationMatrix;
                //model = rotationMatrixX * translationMatrix;


                GL.UniformMatrix4(modelLocation, false, ref model);
                GL.Uniform1(alphaLocation, Math.Abs(70 - Math.Sqrt(i * i + j * j)) / 70.0f);

                GL.DrawElements(PrimitiveType.Triangles, indicesPlane.Length, DrawElementsType.UnsignedInt, 0);

                rotationMatrixX = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0).Normalized(), MathHelper.DegreesToRadians(90));
                rotationMatrixY = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1).Normalized(), MathHelper.DegreesToRadians(-45));
                model = rotationMatrixY * rotationMatrixX * translationMatrix;
                GL.UniformMatrix4(modelLocation, false, ref model);
                GL.Uniform1(alphaLocation, Math.Abs(70 - Math.Sqrt(i * i + j * j)) / 70);

                GL.DrawElements(PrimitiveType.Triangles, indicesPlane.Length, DrawElementsType.UnsignedInt, 0);

            }
        }
        GL.BindVertexArray(0);
    }
}
public class CubeStorage
{
    public readonly HashSet<Cube> allCubes = new();
    private readonly Dictionary<(int, int), List<Cube>> cubeGrid = new();
    private readonly int chunkSize = 10;

    public bool Add(Cube cube)
    {
        if (!allCubes.Add(cube)) return false;
        var key = ((int)(cube.X / chunkSize), (int)(cube.Z / chunkSize));
        if (!cubeGrid.ContainsKey(key)) cubeGrid[key] = new();
        cubeGrid[key].Add(cube);
        return true;
    }

    public void AddMultiple(IEnumerable<Cube> cubes)
    {
        foreach (var cube in cubes)
            allCubes.Add(cube);
    }
    public IEnumerable<Cube> GetCubesNear(float x, float z, float radius = 100f)
    {
        return allCubes.Where(cube =>
        Math.Abs(cube.X - x) <= radius &&
        Math.Abs(cube.Z - z) <= radius);
    }
}
public class PlaneStorage
{
    public readonly HashSet<Plane> allPlanes = new();
    private readonly Dictionary<(int, int), List<Plane>> planeGrid = new();
    private readonly int chunkSize = 10;

    public bool Add(Plane plane)
    {
        if (!allPlanes.Add(plane)) return false;
        var key = ((int)(plane.X / chunkSize), (int)(plane.Z / chunkSize));
        if (!planeGrid.ContainsKey(key)) planeGrid[key] = new();
        planeGrid[key].Add(plane);
        return true;
    }

    public void AddMultiple(IEnumerable<Plane> planes)
    {
        foreach (var plane in planes)
            allPlanes.Add(plane);
    }
    public IEnumerable<Plane> GetPlanesNear(float x, float z, float radius = 100f)
    {
        return allPlanes.Where(plane =>
        Math.Abs(plane.X - x) <= radius &&
        Math.Abs(plane.Z - z) <= radius);
    }
}
public class RoadGenerator
{
    private readonly Random rand = new Random();
    private readonly int roadWidth = 5;
    private readonly float blockSize = 3.0f;
    private readonly float roadY = -2.8f;

    // Храним сгенерированные координаты дороги как множества для быстрой проверки
    public HashSet<(int x, int z)> RoadTiles { get; private set; } = new();

    // Генерация замкнутой дороги
    public void GenerateLoopedRoad(int mapMin = -1000, int mapMax = 1000)
    {
        RoadTiles.Clear();

        List<(int x, int z)> controlPoints = new();
        int centerX = 0;
        int centerZ = 0;
        int radius = 100;
        int points = 20;

        for (int i = 0; i < points; i++)
        {
            double angle = i * 2 * Math.PI / points;
            int x = centerX + (int)((radius + rand.Next(-50, 50)) * Math.Cos(angle));
            int z = centerZ + (int)((radius + rand.Next(-50, 50)) * Math.Sin(angle));
            controlPoints.Add((x, z));
        }

        for (int i = 0; i < controlPoints.Count; i++)
        {
            var from = controlPoints[i];
            var to = controlPoints[(i + 1) % controlPoints.Count];
            DrawRoadSegment(from.x, from.z, to.x, to.z);
        }
        DrawRoadSegment(0, 0, controlPoints[0].x, controlPoints[0].z);
    }

    private void DrawRoadSegment(int x0, int z0, int x1, int z1)
    {
        int dx = Math.Abs(x1 - x0);
        int dz = Math.Abs(z1 - z0);
        int sx = x0 < x1 ? 1 : -1;
        int sz = z0 < z1 ? 1 : -1;
        int err = dx - dz;

        while (true)
        {
            AddRoadTileWithWidth(x0, z0);
            if (x0 == x1 && z0 == z1) break;
            int e2 = 2 * err;
            if (e2 > -dz) { err -= dz; x0 += sx; }
            if (e2 < dx) { err += dx; z0 += sz; }
        }
    }

    private void AddRoadTileWithWidth(int x, int z)
    {
        int half = roadWidth / 2;
        for (int dx = -half; dx <= half; dx++)
        {
            for (int dz = -half; dz <= half; dz++)
            {
                if (dx * dx + dz * dz <= half * half)
                {
                    RoadTiles.Add((x + dx, z + dz));
                }
            }
        }
    }

    public bool IsOnRoad(int x, int z) => RoadTiles.Contains((x, z));

    // Возвращаем кубы для отрисовки
    public List<Cube> GetRoadCubes()
    {
        return RoadTiles.Select(tile =>
        {
            var cube = new Cube(
                tile.x * blockSize,
                roadY,
                tile.z * blockSize,
                blockSize,
                0f, 0f, 0f, 0f // Без вращения
            );

            // Цвет: основной — тёмно-серый, альтернативный — чуть светлее
            cube.SetColorI(64, 64, 64, 64, 64, 64);
            return cube;
        }).ToList();
    }
}
public class TreePlacer
{
    private readonly Random _random = new();
    public HashSet<(int x, int z)> OccupiedTiles { get; private set; } = new();

    private Dictionary<string, int> TreeCrowns = new()
    {
        {"BigSpruce", 8},
        {"Spruce", 5},
        {"SmallBrich", 3},
        {"Brich", 5},
        {"BigPine", 10},
        {"Pine", 7},
        {"BigOak", 8},
        {"Oak", 5}
    };

    public Tree CreateTree(string treeType, int x, int z)
    {
        switch (treeType)
        {
            case "BigSpruce":
                {
                    var t = new BigSpruce(x, 0, z);
                    return t;
                }
            case "Spruce":
                {
                    var t = new Spruce(x, 0, z);
                    return t;
                }
            case "SmallBrich":
                {
                    var t = new SmallBrich(x, 0, z);
                    return t;
                }
            case "Brich":
                {
                    var t = new Brich(x, 0, z);
                    return t;
                }
            case "BigPine":
                {
                    var t = new BigPine(x, 0, z);
                    return t;
                }
            case "Pine":
                {
                    var t = new Pine(x, 0, z);
                    return t;
                }
            case "BigOak":
                {
                    var t = new BigOak(x, 0, z);
                    return t;
                }
            case "Oak":
                {
                    var t = new Oak(x, 0, z);
                    return t;
                }
            default:
                throw new ArgumentException("Unknown tree type");
        }
    }

    public List<Tree> PlaceTrees(int count, int mapSize, HashSet<(int x, int z)> RoadTiles)
    {
        List<Tree> placedTrees = new();

        for (int i = 0; i < count; i++)
        {
            bool placed = false;
            int attempts = 0;
            while (!placed && attempts++ < 1000)
            {
                string treeType = TreeCrowns.Keys.ElementAt(_random.Next(TreeCrowns.Count));
                int radius = (TreeCrowns[treeType] / 2) + 1;
                int x = _random.Next(-mapSize + radius, mapSize - radius);
                int z = _random.Next(-mapSize + radius, mapSize - radius);

                if (CanPlaceTree(x, z, radius, RoadTiles))
                {
                    MarkOccupied(x, z, radius);
                    Tree t = CreateTree(treeType, x, z);
                    placedTrees.Add(t);
                    placed = true;
                }
            }
        }
        return placedTrees;
    }

    private bool CanPlaceTree(int x, int z, int radius, HashSet<(int x, int z)> RoadTiles)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dz = -radius; dz <= radius; dz++)
            {
                int nx = x + dx;
                int nz = z + dz;
                if (RoadTiles.Contains((nx, nz)) || RoadTiles.Contains((nx - 1, nz)) || RoadTiles.Contains((nx + 1, nz)) || RoadTiles.Contains((nx, nz - 1)) || RoadTiles.Contains((nx, nz + 1)) || RoadTiles.Contains((nx + 1, nz + 1)) || RoadTiles.Contains((nx - 1, nz - 1)) || RoadTiles.Contains((nx - 1, nz + 1)) || RoadTiles.Contains((nx + 1, nz - 1)) || OccupiedTiles.Contains((nx, nz)))
                    return false;
            }
        }
        return true;
    }

    private void MarkOccupied(int x, int z, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dz = -radius; dz <= radius; dz++)
            {
                OccupiedTiles.Add((x + dx, z + dz));
            }
        }
    }
}
public class Camera
{
    public Vector3 Position { get; set; }
    public Vector3 Front { get; set; } = new Vector3(0.0f, 0.0f, 1.0f);
    public Vector3 Up { get; set; } = new Vector3(0.0f, 1.0f, 0.0f);

    public float Yaw { get; set; } = 180.0f; // Горизонтальный угол (в градусах)
    public float Pitch { get; set; } = -30.0f; // Вертикальный угол (в градусах)
    public float Speed { get; set; } = 20.5f;
    public float Sensitivity { get; set; } = 0.1f;
    public Camera(Vector3 position)
    {
        Position = position;
    }
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + Front, Up);
    }
}
public class Coquette
{
    public float X;
    public float Z;
    public float Angle;
    public float Speed;
    public float RotationSpeed;
    List<Cube> Cubes = new();
    public static uint[] indicesCube = new uint[]
    {
    0, 1, 2, 2, 3, 0,    // Передняя грань
    4, 5, 6, 6, 7, 4,    // Задняя грань
    8, 9, 10, 10, 11, 8,  // Левая грань
    12, 13, 14, 14, 15, 12, // Правая грань
    16, 17, 18, 18, 19, 16, // Верхняя грань
    20, 21, 22, 22, 23, 20  // Нижняя грань
    };

    public class Plate
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Plate(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
        }
    }
    public class Stairs1
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs1(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class Stairs2
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs2(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class Stairs3
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs3(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class Stairs4
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs4(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }

    public class Stairs5
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs5(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class Stairs6
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs6(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class Stairs7
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public Stairs7(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
            Cube c6 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class halfStairs1
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public halfStairs1(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c5 = new Cube(X - 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c5.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c5);
        }
    }
    public class halfStairs2
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public halfStairs2(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c6 = new Cube(X - 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class halfStairs3
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public halfStairs3(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c6 = new Cube(X + 0.25f, Y + 0.5f, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }
    public class halfStairs4
    {
        float X;
        float Y;
        float Z;
        public List<Cube> Cubes { get; } = new();

        public halfStairs4(float x, float y, float z, int R, int G, int B)
        {
            X = x; Y = y; Z = z;
            Cube c1 = new Cube(X - 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c1.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c1);
            Cube c2 = new Cube(X - 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c2.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c2);
            Cube c3 = new Cube(X + 0.25f, Y, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c3.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c3);
            Cube c4 = new Cube(X + 0.25f, Y, Z + 0.25f, 0.5f, 0, 1, 0, 0);
            c4.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c4);
            Cube c6 = new Cube(X + 0.25f, Y + 0.5f, Z - 0.25f, 0.5f, 0, 1, 0, 0);
            c6.SetColorI(R, G, B, R, G, B);
            Cubes.Add(c6);
        }
    }

    public Coquette(float x = 0, float z = 0)
    {
        X = x;
        Z = z;
        Cube c1 = new Cube(0.5f, 1.5f, 1.5f, 1, 0, 1, 0, 0);
        c1.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c1);
        Cube c2 = new Cube(1.5f, 1.5f, 1.5f, 1, 0, 1, 0, 0);
        c2.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c2);
        Cube c3 = new Cube(2.5f, 1.5f, 1.5f, 1, 0, 1, 0, 0);
        c3.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c3);
        Cube c4 = new Cube(-0.5f, 1.5f, 1.5f, 1, 0, 1, 0, 0);
        c4.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c4);
        Cube c5 = new Cube(0.5f, 1.5f, -1.5f, 1, 0, 1, 0, 0);
        c5.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c5);
        Cube c6 = new Cube(1.5f, 1.5f, -1.5f, 1, 0, 1, 0, 0);
        c6.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c6);
        Cube c7 = new Cube(2.5f, 1.5f, -1.5f, 1, 0, 1, 0, 0);
        c7.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c7);
        Cube c8 = new Cube(-0.5f, 1.5f, -1.5f, 1, 0, 1, 0, 0);
        c8.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c8);
        Plate p1 = new Plate(0.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p1.Cubes);
        Plate p2 = new Plate(1.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p2.Cubes);
        Plate p3 = new Plate(2.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p3.Cubes);
        Plate p4 = new Plate(-0.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p4.Cubes);
        Plate p5 = new Plate(0.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p5.Cubes);
        Plate p6 = new Plate(1.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p6.Cubes);
        Plate p7 = new Plate(2.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p7.Cubes);
        Plate p8 = new Plate(-0.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p8.Cubes);
        Plate p9 = new Plate(-1.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p9.Cubes);
        Plate p10 = new Plate(-1.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p10.Cubes);
        Stairs1 s1 = new Stairs1(-1.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s1.Cubes);
        Stairs2 s2 = new Stairs2(-1.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s2.Cubes);
        Stairs1 s3 = new Stairs1(-2.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s3.Cubes);
        Stairs2 s4 = new Stairs2(-2.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s4.Cubes);
        Stairs4 s5 = new Stairs4(-3.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s5.Cubes);
        Stairs3 s6 = new Stairs3(-3.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s6.Cubes);
        Stairs4 s7 = new Stairs4(-4.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s7.Cubes);
        Stairs3 s8 = new Stairs3(-4.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s8.Cubes);
        Cube c9 = new Cube(-2.5f, 0.5f, -1.5f, 1, 0, 1, 0, 0);
        c9.SetColorI(49, 49, 49, 49, 49, 49);
        Cubes.Add(c9);
        Cube c10 = new Cube(-2.5f, 0.5f, 1.5f, 1, 0, 1, 0, 0);
        c10.SetColorI(49, 49, 49, 49, 49, 49);
        Cubes.Add(c10);
        Plate p11 = new Plate(-3.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p11.Cubes);
        Plate p12 = new Plate(-3.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p12.Cubes);
        Plate p13 = new Plate(-4.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p13.Cubes);
        Plate p14 = new Plate(-4.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p14.Cubes);
        Stairs2 s9 = new Stairs2(-5.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s9.Cubes);
        Stairs1 s10 = new Stairs1(-5.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s10.Cubes);
        Stairs5 s11 = new Stairs5(3.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s11.Cubes);
        Stairs5 s12 = new Stairs5(3.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s12.Cubes);
        Plate p15 = new Plate(4.5f, 1.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p15.Cubes);
        Plate p16 = new Plate(4.5f, 1.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p16.Cubes);
        Stairs4 s13 = new Stairs4(5.5f, 1.25f, -1.5f, 102, 205, 190);
        Cubes.AddRange(s13.Cubes);
        Stairs3 s14 = new Stairs3(5.5f, 1.25f, 1.5f, 102, 205, 190);
        Cubes.AddRange(s14.Cubes);
        Plate p17 = new Plate(5.5f, 0.75f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p17.Cubes);
        Plate p18 = new Plate(5.5f, 0.75f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p18.Cubes);
        Cube c11 = new Cube(4.5f, 0.5f, -1.5f, 1, 0, 1, 0, 0);
        c11.SetColorI(49, 49, 49, 49, 49, 49);
        Cubes.Add(c11);
        Cube c12 = new Cube(4.5f, 0.5f, 1.5f, 1, 0, 1, 0, 0);
        c12.SetColorI(49, 49, 49, 49, 49, 49);
        Cubes.Add(c12);
        Plate p19 = new Plate(5.55f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p19.Cubes);
        Plate p20 = new Plate(5.55f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p20.Cubes);
        Cube c13 = new Cube(5.55f, 1.5f, -0.5f, 1, 0, 1, 0, 0);
        c13.SetColorI(142, 142, 143, 142, 142, 143);
        Cubes.Add(c13);
        Cube c14 = new Cube(5.55f, 1.5f, 0.5f, 1, 0, 1, 0, 0);
        c14.SetColorI(143, 142, 143, 142, 142, 143);
        Cubes.Add(c14);
        Cube c15 = new Cube(4.5f, 1.625f, -0.5f, 1, 0, 1, 0, 0);
        c15.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c15);
        Cube c16 = new Cube(4.5f, 1.625f, 0.5f, 1, 0, 1, 0, 0);
        c16.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c16);
        Cube c17 = new Cube(5.49f, 1.625f, -0.5f, 1, 0, 1, 0, 0);
        c17.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c17);
        Cube c18 = new Cube(5.49f, 1.625f, 0.5f, 1, 0, 1, 0, 0);
        c18.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c18);
        Cube c19 = new Cube(3.5f, 1.625f, -0.5f, 1, 0, 1, 0, 0);
        c19.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c19);
        Cube c20 = new Cube(3.5f, 1.625f, 0.5f, 1, 0, 1, 0, 0);
        c20.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c20);
        Cube c21 = new Cube(4.5f, 1.5f, -0.5f, 1, 0, 1, 0, 0);
        c21.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c21);
        Cube c22 = new Cube(4.5f, 1.5f, 0.5f, 1, 0, 1, 0, 0);
        c22.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c22);
        Cube c23 = new Cube(3.5f, 1.5f, -0.5f, 1, 0, 1, 0, 0);
        c23.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c23);
        Cube c24 = new Cube(3.5f, 1.5f, 0.5f, 1, 0, 1, 0, 0);
        c24.SetColorI(102, 205, 190, 102, 205, 190);
        Cubes.Add(c24);
        Plate p21 = new Plate(4.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p21.Cubes);
        Plate p22 = new Plate(4.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p22.Cubes);
        Plate p23 = new Plate(3.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p23.Cubes);
        Plate p24 = new Plate(3.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p24.Cubes);
        Plate p25 = new Plate(2.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p25.Cubes);
        Plate p26 = new Plate(2.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p26.Cubes);
        Plate p27 = new Plate(1.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p27.Cubes);
        Plate p28 = new Plate(1.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p28.Cubes);
        Plate p29 = new Plate(0.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p29.Cubes);
        Plate p30 = new Plate(0.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p30.Cubes);
        Plate p31 = new Plate(-0.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p31.Cubes);
        Plate p32 = new Plate(-0.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p32.Cubes);
        Plate p33 = new Plate(-1.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p33.Cubes);
        Plate p34 = new Plate(-1.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p34.Cubes);
        Plate p35 = new Plate(-2.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p35.Cubes);
        Plate p36 = new Plate(-2.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p36.Cubes);
        Plate p37 = new Plate(-3.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p37.Cubes);
        Plate p38 = new Plate(-3.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p38.Cubes);
        Plate p39 = new Plate(-4.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p39.Cubes);
        Plate p40 = new Plate(-4.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p40.Cubes);
        Plate p41 = new Plate(-5.5f, 0.75f, 0.5f, 120, 120, 128);
        Cubes.AddRange(p41.Cubes);
        Plate p42 = new Plate(-5.5f, 0.75f, -0.5f, 120, 120, 128);
        Cubes.AddRange(p42.Cubes);
        Stairs6 s15 = new Stairs6(2.5f, 2.25f, -0.5f, 49, 49, 49);
        Cubes.AddRange(s15.Cubes);
        Stairs6 s16 = new Stairs6(2.5f, 2.25f, 0.5f, 49, 49, 49);
        Cubes.AddRange(s16.Cubes);
        Stairs1 s17 = new Stairs1(1.5f, 2.25f, -1.5f, 49, 49, 49);
        Cubes.AddRange(s17.Cubes);
        Stairs2 s18 = new Stairs2(1.5f, 2.25f, 1.5f, 49, 49, 49);
        Cubes.AddRange(s18.Cubes);
        Stairs1 s19 = new Stairs1(0.5f, 2.25f, -1.5f, 49, 49, 49);
        Cubes.AddRange(s19.Cubes);
        Stairs2 s20 = new Stairs2(0.5f, 2.25f, 1.5f, 49, 49, 49);
        Cubes.AddRange(s20.Cubes);
        Stairs1 s21 = new Stairs1(-0.5f, 2.25f, -1.5f, 49, 49, 49);
        Cubes.AddRange(s21.Cubes);
        Stairs2 s22 = new Stairs2(-0.5f, 2.25f, 1.5f, 49, 49, 49);
        Cubes.AddRange(s22.Cubes);
        Stairs1 s23 = new Stairs1(-1.5f, 2.25f, -1.5f, 220, 220, 220);
        Cubes.AddRange(s23.Cubes);
        Stairs2 s24 = new Stairs2(-1.5f, 2.25f, 1.5f, 220, 220, 220);
        Cubes.AddRange(s24.Cubes);
        halfStairs1 h1 = new halfStairs1(2.5f, 2.25f, -1.5f, 49, 49, 49);
        Cubes.AddRange(h1.Cubes);
        halfStairs2 h2 = new halfStairs2(2.5f, 2.25f, 1.5f, 49, 49, 49);
        Cubes.AddRange(h2.Cubes);
        halfStairs3 h3 = new halfStairs3(-2.5f, 2.25f, -1.5f, 220, 220, 220);
        Cubes.AddRange(h3.Cubes);
        halfStairs4 h4 = new halfStairs4(-2.5f, 2.25f, 1.5f, 220, 220, 220);
        Cubes.AddRange(h4.Cubes);
        Stairs7 s25 = new Stairs7(-2.5f, 2.25f, -0.5f, 49, 49, 49);
        Cubes.AddRange(s25.Cubes);
        Stairs7 s26 = new Stairs7(-2.5f, 2.25f, 0.5f, 49, 49, 49);
        Cubes.AddRange(s26.Cubes);
        Cube c25 = new Cube(1.5f, 2.625f, -0.5f, 1, 0, 1, 0, 0);
        c25.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c25);
        Cube c26 = new Cube(1.5f, 2.625f, 0.5f, 1, 0, 1, 0, 0);
        c26.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c26);
        Cube c27 = new Cube(-1.5f, 2.625f, -0.5f, 1, 0, 1, 0, 0);
        c27.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c27);
        Cube c28 = new Cube(-1.5f, 2.625f, 0.5f, 1, 0, 1, 0, 0);
        c28.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c28);
        Cube c29 = new Cube(0.5f, 2.75f, -0.5f, 1, 0, 1, 0, 0);
        c29.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c29);
        Cube c30 = new Cube(0.5f, 2.75f, 0.5f, 1, 0, 1, 0, 0);
        c30.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c30);
        Cube c31 = new Cube(-0.5f, 2.75f, -0.5f, 1, 0, 1, 0, 0);
        c31.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c31);
        Cube c32 = new Cube(-0.5f, 2.75f, 0.5f, 1, 0, 1, 0, 0);
        c32.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c32);
        Cube c33 = new Cube(-3.5f, 1.5f, 0.5f, 1, 0, 1, 0, 0);
        c33.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c33);
        Cube c34 = new Cube(-3.5f, 1.5f, -0.5f, 1, 0, 1, 0, 0);
        c34.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c34);
        Cube c35 = new Cube(-4.5f, 1.01f, 0.5f, 1, 0, 1, 0, 0);
        c35.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c35);
        Cube c36 = new Cube(-4.5f, 1.01f, -0.5f, 1, 0, 1, 0, 0);
        c36.SetColorI(220, 220, 220, 220, 220, 220);
        Cubes.Add(c36);
        Plate p43 = new Plate(-5.49f, 1.0f, 0.49f, 220, 220, 220);
        Cubes.AddRange(p43.Cubes);
        Plate p44 = new Plate(-5.49f, 1.0f, -0.49f, 220, 220, 220);
        Cubes.AddRange(p44.Cubes);
        Plate p45 = new Plate(-5.5f, 1.0f, 1.5f, 102, 205, 190);
        Cubes.AddRange(p45.Cubes);
        Plate p46 = new Plate(-5.5f, 1.0f, -1.5f, 102, 205, 190);
        Cubes.AddRange(p46.Cubes);
        Cube c37 = new Cube(-2.5f, 0.5f, -1.76f, 0.6f, 0, 1, 0, 0);
        c37.SetColorI(120, 120, 120, 49, 49, 49);
        Cubes.Add(c37);
        Cube c38 = new Cube(-2.5f, 0.5f, 1.76f, 0.6f, 0, 1, 0, 0);
        c38.SetColorI(120, 120, 120, 49, 49, 49);
        Cubes.Add(c38);
        Cube c39 = new Cube(4.5f, 0.5f, -1.76f, 0.6f, 0, 1, 0, 0);
        c39.SetColorI(120, 120, 120, 49, 49, 49);
        Cubes.Add(c39);
        Cube c40 = new Cube(4.5f, 0.5f, 1.76f, 0.6f, 0, 1, 0, 0);
        c40.SetColorI(120, 120, 120, 49, 49, 49);
        Cubes.Add(c40);
        Cube c41 = new Cube(5.86f, 1.75f, -1.75f, 0.35f, 0, 1, 0, 0);
        c41.SetColorI(210, 210, 99, 210, 210, 99);
        Cubes.Add(c41);
        Cube c42 = new Cube(5.86f, 1.75f, 1.75f, 0.35f, 0, 1, 0, 0);
        c42.SetColorI(210, 210, 99, 210, 210, 99);
        Cubes.Add(c42);
        Cube c43 = new Cube(5.86f, 1.75f, -1.25f, 0.35f, 0, 1, 0, 0);
        c43.SetColorI(210, 210, 99, 210, 210, 99);
        Cubes.Add(c43);
        Cube c44 = new Cube(5.86f, 1.75f, 1.25f, 0.35f, 0, 1, 0, 0);
        c44.SetColorI(210, 210, 99, 210, 210, 99);
        Cubes.Add(c44);
    }
    public void Update(float DeltaTime)
    {
        Angle += RotationSpeed * DeltaTime;

        float radians = -MathHelper.DegreesToRadians(Angle);
        Vector3 direction = new Vector3((float)Math.Cos(radians), 0, (float)Math.Sin(radians));

        // Обновляем позицию машины
        Vector3 velocity = direction * Speed;
        X += (velocity * DeltaTime).X;
        Z += (velocity * DeltaTime).Z;
    }
    public void Load()
    {
        foreach (var cube in Cubes)
        {
            cube.Load();
        }
    }
    public void Draw(int shaderProgram, int modelLocation, int alphaLocation)
    {
        Matrix4 objectRotation = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0).Normalized(), MathHelper.DegreesToRadians(Angle));
        Matrix4 objectTranslation = Matrix4.CreateTranslation(new Vector3(X, 0.2f, Z));
        Matrix4 objectTransform = objectRotation * objectTranslation;
        GL.UseProgram(shaderProgram);
        foreach (var cube in Cubes)
        {
            GL.BindVertexArray(cube.VAO);
            Matrix4 localTranslation = Matrix4.CreateTranslation(new Vector3(cube.X, cube.Y, cube.Z));
            Matrix4 model = localTranslation * objectTransform;
            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.Uniform1(alphaLocation, 1f);

            GL.DrawElements(PrimitiveType.Triangles, indicesCube.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

    }
}