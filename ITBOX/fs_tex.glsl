#version 330

in vec2 f_texcoord;
out vec4 outputColor;

uniform sampler2D maintexture;

void
main()
{
	vec2 flipped_texcoord = vec2(f_texcoord.x, f_texcoord.y);
    outputColor = texture(maintexture, flipped_texcoord);
}