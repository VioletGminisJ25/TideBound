@tool
class_name Water2D
extends Sprite2D

var drop_index: int = 0
var area2d: Area2D
var collision_shape: CollisionShape2D
var owner_id: int
var area_rid: RID

enum SURFACE_TYPES {
	PLAIN,
	GRADIENT
}
@export var automatic_impact: bool = true
@export var surface_type: SURFACE_TYPES = SURFACE_TYPES.GRADIENT : set = set_surface_type

@export var deformation_speed_1: Vector2 = Vector2(1.0, 1.0) : set = set_deformation_speed_1
@export var deformation_speed_2: Vector2 = Vector2(-2.0, -2.0) :set =  set_deformation_speed_2
@export var deformation_strength: Vector2 = Vector2(1.0, 1.0) : set = set_deformation_strength
@export var deformation_size: Vector2 = Vector2(1.0, 1.0) : set =  set_deformation_size

@export var water_level: float = 20.0 :  set = set_water_level
@export var water_color: Color = Color(0.0, 0.0,1.0, 0.75) : set =  set_water_color

@export var surface_width: float = 2.0 : set =  set_surface_width
@export var surface_color: Color = Color(1.0, 1.0,1.0, 0.75) : set = set_surface_color
@export var surface_deformation_strength: float = 1.0 : set = set_surface_deformation_strength

@export var wave_speed: float = 1.0 : set = set_wave_speed
@export var wave_distance_attenuation: float = 1.0 : set = set_wave_distance_attenuation
@export var wave_time_attenuation: float = 1.0 : set =  set_wave_time_attenuation

signal water_entered(water, impact_pos, body_id, body, body_shape, area_shape)
signal water_exited(water, impact_pos, body_id, body, body_shape, area_shape)
signal water_splashed(water, impact_pos, body_id, body, body_shape, area_shape)

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	texture = preload("icon.png")
	material = preload("water2D_material.tres")
	add_shape()

func add_shape():
	if not Engine.is_editor_hint():
		area2d = Area2D.new()
		collision_shape = CollisionShape2D.new()
		adjust_water_area()
		area2d.add_child(collision_shape)
		area2d.connect("area_shape_entered", self._on_Area2D_body_shape_entered)
		area2d.connect("body_shape_entered", self._on_Area2D_body_shape_entered)
		area2d.connect("area_shape_exited", self._on_Area2D_body_shape_exited)
		area2d.connect("body_shape_exited", self._on_Area2D_body_shape_exited)
		add_child(area2d)

func adjust_water_area():
	if not Engine.is_editor_hint():
		var new_rect = RectangleShape2D.new()
		var sprite_size = get_rect().size
		var new_rect_extents = sprite_size / 2
		var half_water_level = material.get_shader_parameter('water_level')/ 2 / scale.y
		new_rect_extents.y -= half_water_level
		if new_rect_extents.y < 0.0 : new_rect_extents.y = 0
		new_rect.extents = new_rect_extents
		collision_shape.set_shape(new_rect)
		area2d.position.y = half_water_level

func _ready() :
	material.set_shader_parameter("scale", scale)

func compute_impact_pos(body):
	var size = get_rect().size * scale
	var sprite_top_left = global_position - size / 2;
	var body_pos_relative_top_left = body.global_position - sprite_top_left
	var surface_y = material.get_shader_parameter("water_level")
	return Vector2(body_pos_relative_top_left.x, surface_y)

func _on_Area2D_body_shape_entered(body_id, body, body_shape, area_shape):
	var impact_pos = compute_impact_pos(body)
	if impact_pos.y <= body.global_position.y :
		if automatic_impact :
			add_impact(impact_pos, 10.0, 5.0)
		emit_signal("water_splashed", self, impact_pos, body_id, body, body_shape, area_shape)
	emit_signal("water_entered", self, impact_pos, body_id, body, body_shape, area_shape)

func _on_Area2D_body_shape_exited(body_id, body, body_shape, area_shape):
	var impact_pos = compute_impact_pos(body)
	emit_signal("water_exited", self, impact_pos, body_id, body, body_shape, area_shape)

func add_impact(pos, length, height):
	material.set_shader_parameter("impact_height_" + str(drop_index), height)
	material.set_shader_parameter("impact_length_" + str(drop_index), length)
	material.set_shader_parameter("impact_pos_" + str(drop_index), pos)
	material.set_shader_parameter("impact_time_" + str(drop_index), Time.get_ticks_msec() / 1000.0)
	drop_index = (drop_index + 1) % 8

func _process(_delta):
	var current_time = Time.get_ticks_msec() / 1000.0
	material.set_shader_parameter("current_time", current_time)
	if Engine.is_editor_hint():
		if not area2d:
			add_shape()
		material.set_shader_parameter("scale", scale)
		adjust_water_area()

func _on_Water2D_item_rect_changed():
	adjust_water_area()
	material.set_shader_parameter("scale", scale)

func set_surface_type(_value):
	surface_type = _value
	material.set_shader_parameter("surface_type", _value)

func set_deformation_speed_1( _value ):
	deformation_speed_1 = _value
	material.set_shader_parameter("deformation_speed_1", _value)

func set_deformation_speed_2( _value ):
	deformation_speed_2 = _value
	material.set_shader_parameter("deformation_speed_2", _value)

func set_deformation_strength( _value ):
	deformation_strength = _value
	material.set_shader_parameter("deformation_strength", _value)

func set_deformation_size( _value ):
	deformation_size = _value
	material.set_shader_parameter("tile_factor", Vector2(1.0 / _value.x, 1.0 / _value.y))

func set_water_level( _value ):
	water_level = _value
	material.set_shader_parameter("water_level", _value)

func set_water_color( _value ):
	water_color = _value
	material.set_shader_parameter("water_color", _value)

func set_surface_width( _value ):
	surface_width = _value
	material.set_shader_parameter("surface_width", _value)

func set_surface_color( _value ):
	surface_color = _value
	material.set_shader_parameter("surface_color", _value)

func set_surface_deformation_strength( _value ):
	surface_deformation_strength = _value
	material.set_shader_parameter("surface_deformation_strength", _value)

func set_wave_speed( _value ):
	wave_speed = _value
	material.set_shader_parameter("wave_speed", _value)

func set_wave_distance_attenuation( _value ):
	wave_distance_attenuation = _value
	material.set_shader_parameter("wave_distance_attenuation", _value)

func set_wave_time_attenuation( _value ):
	wave_time_attenuation = _value
	material.set_shader_parameter("wave_time_attenuation", _value)


