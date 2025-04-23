package com.amipem.mono.repositorios;

import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;

import com.amipem.mono.entidades.Grabacion;

public interface GrabacionCRUDRepository extends CrudRepository<Grabacion, Integer> {
	
	@Query("from Grabacion as g where g.orden=:orden and g.lote=:lote")
	public List<Grabacion> getCountOrder(String orden,int lote);

}
