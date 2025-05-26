package com.amipem.mono.controladores;

import java.util.ArrayList;
import java.sql.Date;
import java.time.LocalDateTime;
import java.time.OffsetDateTime;
import java.util.GregorianCalendar;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import com.amipem.mono.MonoApplication;
import com.amipem.mono.clases.Contador;
import com.amipem.mono.clases.GrabacionDTO;
import com.amipem.mono.clases.RespuestaContador;
import com.amipem.mono.entidades.Grabacion;
import com.amipem.mono.entidades.Orden;
import com.amipem.mono.repositorios.GrabacionCRUDRepository;
import com.amipem.mono.repositorios.OrdenCRUDRepository;

import lombok.Data;

@Data
@RestController
@RequestMapping("Ordenes")
public class GrabacionRestController {

    private final MonoApplication monoApplication;

	@Autowired
	private GrabacionCRUDRepository grabacionCrudRepository;

	@Autowired
	private OrdenCRUDRepository ordenCrudRepository;

    GrabacionRestController(MonoApplication monoApplication) {
        this.monoApplication = monoApplication;
    }

	@PostMapping("leerContador")
	public Contador getContador(@RequestBody Entrada entrada) {
		List<Grabacion> grabaciones = getGrabacionCrudRepository().getCountOrder(entrada.getCodigo());
		return new Contador(grabaciones.size(), entrada.getCodigo());
	}

	@PostMapping("leerOrden")
	public Orden getOrden(@RequestBody Entrada entrada) {

		return getOrdenCrudRepository().findByCodigo(entrada.getCodigo());
	}

	@PostMapping("leerTagsOrden")
	public RespuestaContador getTagsFromOrden(@RequestBody Entrada entrada) {
		
		RespuestaContador respuestaContador=  new RespuestaContador();
		List<Grabacion> grabaciones=getGrabacionCrudRepository().getTagsByOrden(entrada.getCodigo());
		respuestaContador.setGrabaciones(grabaciones);
		return respuestaContador;

	}

	@PostMapping("grabarTagContador")
	public RespuestaContador grabaTag(@RequestBody GrabacionDTO grabacionDTO) {
		Orden orden = getOrdenCrudRepository().findByCodigo(grabacionDTO.getOrden());

		// if(grabaciones.size()>=orden.getCantidad()) {
		List<Grabacion> grabaciones = getGrabacionCrudRepository().getTagsByOrden(grabacionDTO.getOrden());
		if (grabaciones.size() < orden.getCantidad()) {
			Grabacion grabacion = new Grabacion(0, orden, grabacionDTO.getTag(), grabacionDTO.getLinea(),
					OffsetDateTime.now());
			getGrabacionCrudRepository().save(grabacion);
			if (grabacion.getId() > 0)
				grabaciones.add(grabacion);
		}
		RespuestaContador respuestaContador= new RespuestaContador();
		respuestaContador.setGrabaciones(grabaciones);
		return respuestaContador;
	}

	@PostMapping("grabarOrden")
	public Orden grabaOrden(@RequestBody Orden orden) {
		orden.setId(0);
		return getOrdenCrudRepository().save(orden);
	}
	
	@PostMapping("borraUltimaGrabacion/{epc}")
	public void borraUltimaGrabacion(@PathVariable String epc) {
		
		Grabacion grabacion=getGrabacionCrudRepository().findByTag(epc);
		getGrabacionCrudRepository().delete(grabacion);
	}

	@GetMapping("simulacion")
	public String simulacion() {
		return "";
	}
}
;