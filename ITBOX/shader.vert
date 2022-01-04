#version 330 core
layout (location = 0) in vec3 aPosition;
uniform mat4 position;
uniform mat4 rotate;
uniform mat4 scale;
uniform mat4 ortho;

void main()
{
    gl_Position = vec4(aPosition, 1.0)*position*scale*rotate*ortho;
}