package com.amipem.mono.controladores;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import com.amipem.mono.clases.Contador;
import com.amipem.mono.clases.GrabacionDTO;
import com.amipem.mono.entidades.Grabacion;
import com.amipem.mono.entidades.Orden;
import com.amipem.mono.repositorios.GrabacionCRUDRepository;
import com.amipem.mono.repositorios.OrdenCRUDRepository;

import lombok.Data;

@Data
@RestController
public class GrabacionRestController {

	@Autowired
	private GrabacionCRUDRepository grabacionCrudRepository;
	
	@Autowired
	private OrdenCRUDRepository ordenCrudRepository;
	
	
	
	@PostMapping("leerContador/{orden}/{lote}")
	public Contador getContador( @PathVariable String orden, @PathVariable int lote) {
		List<Grabacion> grabaciones=getGrabacionCrudRepository().getCountOrder(orden, lote);
		return new Contador(grabaciones.size(), orden ,lote);
	}
	
	@PostMapping("leerOrden/{codigo}")
	public Orden getOrden( @PathVariable String codigo) {
		
		return getOrdenCrudRepository().findByCodigo(codigo);
	}
	
	@PostMapping("leerTagsOrden/{orden}")
	public List<Grabacion> getTagsFromOrden(@PathVariable String orden){
		return getGrabacionCrudRepository().getTagsByOrden(orden);
		
	}
	
	@PostMapping("grabarTagContador")
	public List<Grabacion> grabaTag(@RequestBody GrabacionDTO grabacionDTO) {
		Orden orden=getOrdenCrudRepository().findByCodigo(grabacionDTO.getOrden());
		List<Grabacion> grabaciones=getGrabacionCrudRepository().getTagsByOrden(grabacionDTO.getOrden());
		if(grabaciones.size()>=orden.getCantidad()) {
			ArrayList<Grabacion> salida= new ArrayList<Grabacion>();
			Grabacion grabacion= new Grabacion(-1, orden, null, 0, 0);
			salida.add(grabacion);
			return salida;
		}
		else {
		Grabacion grabacion= new Grabacion(0, orden, grabacionDTO.getTag(), grabacionDTO.getLote(),grabacionDTO.getLinea());
		getGrabacionCrudRepository().save(grabacion);
		return getGrabacionCrudRepository().getTagsByOrden(grabacionDTO.getOrden());
		}
	}
	
	@PostMapping("grabarOrden")
	public Orden grabaOrden(@RequestBody Orden orden) {
		return getOrdenCrudRepository().save(orden);
	}
	
	@GetMapping("simulacion")
	public String simulacion() {
		return "";
	}
}
