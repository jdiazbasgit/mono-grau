package com.amipem.mono;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;
import org.springframework.boot.web.servlet.support.SpringBootServletInitializer;

import acceos.recrale.nuevo.Accesos2025Application;

@SpringBootApplication
public class MonoApplication extends SpringBootServletInitializer{

	public static void main(String[] args) {
		SpringApplication.run(MonoApplication.class, args);
	}

	@Override
    protected SpringApplicationBuilder configure(SpringApplicationBuilder application) 
    {
        return application.sources(MonoApplication.class);
    }
}
