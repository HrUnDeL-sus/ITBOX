#version 330

in  vec3 vPosition;
in vec2 texcoord;
out vec2 f_texcoord;
  
uniform mat4 all;

void
main()
{
   f_texcoord = texcoord;
    gl_Position =all*vec4(vPosition, 1.0);
 
}