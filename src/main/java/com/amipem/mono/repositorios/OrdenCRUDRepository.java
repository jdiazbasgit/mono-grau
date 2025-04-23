package com.amipem.mono.repositorios;

import org.springframework.data.repository.CrudRepository;

import com.amipem.mono.entidades.Orden;

public interface OrdenCRUDRepository extends CrudRepository<Orden, Integer> {

	public Orden findByCodigo(String codigo);

}
