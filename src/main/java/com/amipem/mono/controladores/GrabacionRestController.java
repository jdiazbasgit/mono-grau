package com.amipem.mono.controladores;

import java.util.ArrayList;
import java.util.GregorianCalendar;
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

	@PostMapping("leerContador")
	public Contador getContador(@PathVariable String orden) {
		List<Grabacion> grabaciones = getGrabacionCrudRepository().getCountOrder(orden);
		return new Contador(grabaciones.size(), orden);
	}

	@PostMapping("leerOrden/{codigo}")
	public Orden getOrden(@PathVariable String codigo) {

		return getOrdenCrudRepository().findByCodigo(codigo);
	}

	@PostMapping("leerTagsOrden/{orden}")
	public List<Grabacion> getTagsFromOrden(@PathVariable String orden) {
		return getGrabacionCrudRepository().getTagsByOrden(orden);

	}

	@PostMapping("grabarTagContador")
	public List<Grabacion> grabaTag(@RequestBody GrabacionDTO grabacionDTO) {
		Orden orden = getOrdenCrudRepository().findByCodigo(grabacionDTO.getCodigo());

		// if(grabaciones.size()>=orden.getCantidad()) {
		List<Grabacion> grabaciones = getGrabacionCrudRepository().getTagsByOrden(grabacionDTO.getCodigo());
		if (grabaciones.size() < orden.getCantidad()) {
			Grabacion grabacion = new Grabacion(0, orden, grabacionDTO.getTag(), grabacionDTO.getLinea(),
					new GregorianCalendar());
			getGrabacionCrudRepository().save(grabacion);
			if (grabacion.getId() > 0)
				grabaciones.add(grabacion);
		}
		return grabaciones;
	}

	@PostMapping("grabarOrden")
	public Orden grabaOrden(@RequestBody Orden orden) {
		orden.setId(0);
		return getOrdenCrudRepository().save(orden);
	}

	@GetMapping("simulacion")
	public String simulacion() {
		return "";
	}
}
;