/**
*Author: xyhak47
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Tank : MonoBehaviour 
{
    public static Tank Instance = null;
    public Tank()
    {
        Instance = this;
    }

    public int step = 2;
    public bool drawArrow = true;

    public Transform ray_begin;
    public Transform ray_end;

    private void Start()
    {
    }

    public void BeginRotate()
    {
        StartCoroutine(Do());
    }

    private IEnumerator Do()
    {
        int rotate_y = 0;
        int rotate_z = 0;
        int rotate_x = 0;

        int rotate_angle_x = 90;
        int rotate_angle_y = 180;
        int rotate_angle_z = 360;

        for (; rotate_y < rotate_angle_y / step; rotate_y++)
        {
            for (; rotate_z < rotate_angle_z / step; rotate_z++)
            {
                for (; rotate_x < rotate_angle_x / step; rotate_x++)
                {
                    transform.Rotate(-step, 0, 0, Space.Self);

                    //Debug.Log("degree / step : y = " + rotate_y + ", z = " + rotate_z + ", x  =" + rotate_x);
                    //float wait = 1f;
                    //yield return new WaitForSeconds(wait);

                    yield return new WaitForEndOfFrame();

                    Vector3 direction = Calculate_ForwardRayDirection();
                    string pic_name = SaveTexture.Instance.TakePicture();

                    if(drawArrow)
                    {
                        DrawArrow(direction);
                    }

                    DataGenerater.Instance.AppendData(pic_name, direction);
                }

                transform.Rotate(rotate_angle_x, 0, 0, Space.Self);
                rotate_x = 0;

                transform.Rotate(0, 0, -step, Space.Self);
            }

            transform.Rotate(0, 0, rotate_angle_z, Space.Self);
            rotate_z = 0;
        }

        transform.Rotate(0, rotate_angle_y, 0, Space.Self);
        rotate_y = 0;

        DataGenerater.Instance.Save();
    }

    private Vector3 Calculate_ForwardRayDirection()
    {
        //Vector3 ray_begin_screen_space = Camera.main.WorldToScreenPoint(ray_begin.position);
        //Vector3 ray_end_screen_space = Camera.main.WorldToScreenPoint(ray_end.position);
        //Vector3 ray_direction_screen_space = ray_end_screen_space - ray_begin_screen_space;

        Vector3 ray_direction_world_space = ray_end.position - ray_begin.position;
        Vector3 ray_direction_camera_space = Camera.main.worldToCameraMatrix.MultiplyVector(ray_direction_world_space);

        //Debug.Log("ray_direction_camera_space : " + 
        //    ray_direction_camera_space.x + "," + 
        //    ray_direction_camera_space.y + "," + 
        //    ray_direction_camera_space.z);

        return ray_direction_camera_space;
    }

    private void DrawArrow(Vector3 direction_camera)
    {
        Vector3 ray_direction_world_space = Camera.main.cameraToWorldMatrix.MultiplyVector(direction_camera);

        float arrowHeadAngle = 45;
        float arrow_length = 0.005f;
        Vector3 right = Quaternion.LookRotation(ray_direction_world_space) *
            Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, arrow_length);

        Vector3 left = Quaternion.LookRotation(ray_direction_world_space) *
            Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, arrow_length);

        VectorLine.SetCamera3D(Camera.main);

        float line_width = 2f;
        float time = Time.deltaTime * 2;

        VectorLine line = VectorLine.SetLine3D(Color.red, time, new Vector3[] { ray_begin.position, ray_end.position, left, right, ray_end.position });
        line.SetWidth(line_width);
        line.Draw();
    }
}
